using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.FuelComposition;
using SimulationManager.Application.Interfaces;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Exceptions;
using SimulationManager.Domain.Interfaces;

namespace SimulationManager.Application.Services;

public class FuelCompositionService : IFuelCompositionService
{
    private readonly IFuelCompositionRepository _fuelCompositionRepository;
    private readonly ILogger<FuelCompositionService> _logger;

    public FuelCompositionService(
        IFuelCompositionRepository fuelCompositionRepository,
        ILogger<FuelCompositionService> logger)
    {
        _fuelCompositionRepository = fuelCompositionRepository;
        _logger = logger;
    }

    public async Task<PagedResponseDto<FuelCompositionResponseDto>> GetPagedFuelCompositionsAsync(
        int pageNumber,
        int pageSize)
    {
        var (fuelCompositions, totalRecords) = await _fuelCompositionRepository.GetPagedAsync(
                pageNumber,
                pageSize);

        var data = fuelCompositions.Select(MapToResponseDto);

        return new PagedResponseDto<FuelCompositionResponseDto>(
            data,
            pageNumber,
            pageSize,
            totalRecords
        );
    }

    public async Task<FuelCompositionResponseDto?> GetFuelCompositionByIdAsync(int id)
    {
        var fuelComposition = await _fuelCompositionRepository.GetByIdAsync(id);

        return fuelComposition is null ?  null: MapToResponseDto(fuelComposition);
    }

    public async Task<FuelCompositionResponseDto> CreateFuelCompositionAsync(
        CreateFuelCompositionDto dto)
    {
        var fuelComposition = new FuelComposition
        {
            Name = dto.Name,
            Composition = dto.Composition,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await _fuelCompositionRepository.CreateAsync(fuelComposition);

        _logger.LogInformation("FuelComposition {Id} created succesfully", fuelComposition.Id);

        return MapToResponseDto(fuelComposition);

    }

    public async Task<FuelCompositionResponseDto> UpdateFuelCompositionAsync(
        int id,
        UpdateFuelCompositionDto dto)
    {
        var fuelComposition = await _fuelCompositionRepository.GetByIdAsync(id);

        if (fuelComposition is null)
        {
            throw new NotFoundException($"FuelCOmposition with ID {id} not found");
        }

        fuelComposition.Name = dto.Name;
        fuelComposition.Composition = dto.Composition;

        await _fuelCompositionRepository.UpdateAsync(fuelComposition);

        _logger.LogInformation("FuelComposition {Id} updated successfully", fuelComposition.Id);

        return MapToResponseDto(fuelComposition);
    }

    public async Task DeleteFuelCompositionAsync(int id)
    {
        var fuelComposition = await _fuelCompositionRepository.GetByIdAsync(id);

        if (fuelComposition is null)
        {
            throw new NotFoundException($"FuelComposition with ID {id} not found");
        }

        await _fuelCompositionRepository.DeleteAsync(fuelComposition);

        _logger.LogInformation("FuelComposition {Id} deleted sucessfully", id);
    }

    private static FuelCompositionResponseDto MapToResponseDto(FuelComposition fuelComposition)
        => new FuelCompositionResponseDto(
            fuelComposition.Id,
            fuelComposition.Name!,
            fuelComposition.Composition,
            fuelComposition.CreatedAt,
            fuelComposition.UserId);
}
