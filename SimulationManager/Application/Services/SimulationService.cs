using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.Simulation;
using SimulationManager.Application.Interfaces;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Exceptions;
using SimulationManager.Domain.Interfaces;
using System.Text.Json;

namespace SimulationManager.Application.Services;

public class SimulationService : ISimulationService
{
    private readonly ISimulationRepository _simulationRepository;
    private readonly IFuelCompositionRepository _fuelCompositionRepository;
    private readonly ISimulationExternalService _simulationExternalService;
    private readonly ILogger<SimulationService> _logger;

    public SimulationService(
        ISimulationRepository simulationRepository,
        IFuelCompositionRepository fuelCompositionRepository,
        ISimulationExternalService simulationExternalService,
        ILogger<SimulationService> logger)
    {
        _simulationRepository = simulationRepository;
        _fuelCompositionRepository = fuelCompositionRepository;
        _simulationExternalService = simulationExternalService;
        _logger = logger;
    }

    public async Task<PagedResponseDto<SimulationResponseDto>> GetPagedSimulationsAsync(
        int pageNumber,
        int pageSize)
    {
        var (simulations, totalRecords) = await _simulationRepository.GetPagedAsync(
            pageNumber,
            pageSize);

        var data = simulations.Select(MapToResponseDto);

        return new PagedResponseDto<SimulationResponseDto>(
            data,
            pageNumber,
            pageSize,
            totalRecords);
    }

    public async Task<SimulationResponseDto?> GetSimulationByIdAsync(int id)
    {
        var simulation = await _simulationRepository.GetByIdWithResultAsync(id);

        return simulation is null ? null : MapToResponseDto(simulation);
    }

    public async Task<SimulationResponseDto> CreateSimulationAsync(CreateSimulationDto dto)
    {
        // 1. Validate FuelComposition exists
        var fuelComposition = await _fuelCompositionRepository.GetByIdAsync(dto.FuelCompositionId);
        if (fuelComposition is null)
        {
            throw new NotFoundException($"FuelComposition with ID {dto.FuelCompositionId} not found");
        }

        // 2. Create Simulation with Processing status
        var simulation = new Simulation
        {
            Status = "Processing",
            Parameters = dto.Parameters,
            CreatedAt = DateTime.UtcNow,
            UserId = dto.UserId,
            FuelCompositionId = dto.FuelCompositionId
        };

        await _simulationRepository.CreateAsync(simulation);
        _logger.LogInformation("Simulation {Id} created with status Processing", simulation.Id);

        try
        {
            // 3. Execute simulation via FastAPI
            var fastApiResponse = await _simulationExternalService.ExecuteAsync(
                fuelComposition.Composition,
                dto.Parameters);

            // 4. Persist SimulationResult
            var simulationResult = new SimulationResult
            {
                SimulationId = simulation.Id,
                ResultJson = JsonSerializer.Serialize(fastApiResponse),
                CreatedAt = DateTime.UtcNow
            };

            await _simulationRepository.SaveResultAsync(simulationResult);

            // 5. Update simulation status to Completed
            await _simulationRepository.UpdateStatusAsync(simulation.Id, "Completed");

            // 6. Load completed simulation with result
            var completedSimulation = await _simulationRepository.GetByIdWithResultAsync(simulation.Id);

            _logger.LogInformation("Simulation {Id} completed successfully", simulation.Id);

            return MapToResponseDto(completedSimulation!);
        }
        catch (SimulationExecutionException ex)
        {
            _logger.LogError(ex,
                "Simulation {Id} failed. Error: {Error}, Type: {Type}",
                simulation.Id, ex.Message, ex.ErrorType);

            await _simulationRepository.UpdateStatusAsync(simulation.Id, "Failed");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Simulation {Id}", simulation.Id);
            await _simulationRepository.UpdateStatusAsync(simulation.Id, "Failed");
            throw;
        }
    }

    public async Task DeleteSimulationAsync(int id)
    {
        var simulation = await _simulationRepository.GetByIdAsync(id);

        if (simulation is null)
        {
            throw new NotFoundException($"Simulation with ID {id} not found");
        }

        await _simulationRepository.DeleteAsync(simulation);
        _logger.LogInformation("Simulation {Id} deleted successfully", id);
    }

    private static SimulationResponseDto MapToResponseDto(Simulation simulation)
    {
        SimulationResultResponseDto? resultDto = null;

        if (simulation.SimulationResult is not null)
        {
            var resultData = JsonSerializer.Deserialize<FastApiResponseDto>(
                simulation.SimulationResult.ResultJson!);

            resultDto = new SimulationResultResponseDto(
                simulation.SimulationResult.Id,
                resultData!,
                simulation.SimulationResult.CreatedAt);
        }

        return new SimulationResponseDto(
            simulation.Id,
            simulation.Status!,
            simulation.UserId,
            simulation.FuelCompositionId,
            simulation.Parameters,
            simulation.CreatedAt,
            resultDto);
    }
}
