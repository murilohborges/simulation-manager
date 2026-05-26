using Microsoft.AspNetCore.Mvc;
using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.Simulation;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Exceptions;
using SimulationManager.Domain.Interfaces;
using System.Text.Json;

namespace SimulationManager.Api.Controllers;

[Route("api/simulation")]
[ApiController]
public class SimulationController : ControllerBase
{
    private readonly ISimulationRepository _simulationRepository;
    private readonly IFuelCompositionRepository _fuelCompositionRepository;
    private readonly ISimulationExternalService _simulationExternalService;
    private readonly ILogger<SimulationController> _logger;

    public SimulationController(
        ISimulationRepository simulationRepository,
        IFuelCompositionRepository fuelCompositionRepository,
        ISimulationExternalService simulationExternalService,
        ILogger<SimulationController> logger)
    {
        _simulationRepository = simulationRepository;
        _fuelCompositionRepository = fuelCompositionRepository;
        _simulationExternalService = simulationExternalService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<SimulationResponseDto>>> Get(
        [FromQuery] PaginationRequestDto pagination)
    {
        var (simulations, totalRecords) = await _simulationRepository.GetPagedAsync(
            pagination.PageNumber,
            pagination.PageSize);

        var data = simulations.Select(MapToResponseDto);

        return Ok(new PagedResponseDto<SimulationResponseDto>(
            data,
            pagination.PageNumber,
            pagination.PageSize,
            totalRecords));
    }

    [HttpGet("{id:int}", Name = "GetSimulation")]
    public async Task<ActionResult<SimulationResponseDto>> Get(int id)
    {
        var simulation = await _simulationRepository.GetByIdWithResultAsync(id);
        if (simulation is null)
            return NotFound($"Simulation {id} not found.");

        return Ok(MapToResponseDto(simulation));
    }

    [HttpPost]
    public async Task<ActionResult<SimulationResponseDto>> Post(
        [FromBody] CreateSimulationDto dto)
    {
        // 1. Check Fuel Composition
        var fuelComposition = await _fuelCompositionRepository.GetByIdAsync(dto.FuelCompositionId);
        if(fuelComposition is null)
        {
            return NotFound($"Fuel Composition not found.");
        }

        // 2. Persist Simulation with processing status
        var simulation = new Simulation
        {
            Status = "Processing",
            Parameters = dto.Parameters,
            CreatedAt = DateTime.UtcNow,
            UserId = dto.UserId,
            FuelCompositionId = dto.FuelCompositionId
        };

        await _simulationRepository.CreateAsync(simulation);
        _logger.LogInformation("Simulation {Id} created with status Processing.", simulation.Id);

        try
        {
            // 3. Call FastAPI CycleCombCalc
            var fastApiResponse = await _simulationExternalService.ExecuteAsync(
                fuelComposition.Composition,
                dto.Parameters);

            // 4. Persist SimulationResult on your own table
            var simulationResult = new SimulationResult
            {
                SimulationId = simulation.Id,
                ResultJson = JsonSerializer.Serialize(fastApiResponse),
                CreatedAt = DateTime.UtcNow
            };

            await _simulationRepository.SaveResultAsync(simulationResult);

            // 5. Update simulation's status to "Completed"
            await _simulationRepository.UpdateStatusAsync(simulation.Id, "Completed");

            // 6. Loads the complete simulation with the result to return.
            var completedSimulation = await _simulationRepository.GetByIdWithResultAsync(simulation.Id);

            _logger.LogInformation("Simulation {id} completed successfully", simulation.Id);


            return CreatedAtRoute("GetSimulation", new { id = simulation.Id },MapToResponseDto(completedSimulation));
        }
        catch (SimulationExecutionException ex)
        {
            // If FastApi fail, update status to "Failed"
            _logger.LogError(ex,
                "Simulation {Id} failed. Error: {Error}, Type: {Type}",
                simulation.Id, ex.Message, ex.ErrorType);

            await _simulationRepository.UpdateStatusAsync(simulation.Id, "Failed");

            var errorResponse = new
            {
                error = ex.Message,
                type = ex.ErrorType,
                details = ex.ValidationErrors.Select(e => new
                {
                    field = e.Field,
                    message = e.Message,
                    type = e.Type
                }).ToList()
            };

            // Return 422 for validation errors
            if (ex.ErrorType == "RequestValidationError" ||
                ex.ErrorType.Contains("Validation") ||
                ex.ErrorType.Contains("Constraint"))
            {
                return UnprocessableEntity(errorResponse);
            }

            // Return 500 for execution/communication errors
            return StatusCode(500, errorResponse);
        }
        catch (Exception ex)
        {
            // Generic error handling
            _logger.LogError(ex, "Unexpected error in Simulation {id}", simulation.Id);
            await _simulationRepository.UpdateStatusAsync(simulation.Id, "Failed");

            return StatusCode(500, new
            {
                error = "An unexpected error occurred during simulation",
                type = "InternalServerError"
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var simulation = await _simulationRepository.GetByIdAsync(id);

        if (simulation is null)
        {
            return NotFound($"Simulation {id} not found.");
        }

        await _simulationRepository.DeleteAsync(simulation);
        return NoContent();
    }

    // Private mapping method — avoids repetition in endpoints.
    private static SimulationResponseDto MapToResponseDto(Simulation simulation)
    {
        SimulationResultResponseDto? resultDto = null;
        
        if(simulation.SimulationResult is not null)
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
