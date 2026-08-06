using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.FuelComposition;

namespace SimulationManager.Application.Interfaces;

public interface IFuelCompositionService
{
    Task<PagedResponseDto<FuelCompositionResponseDto>> GetPagedFuelCompositionsAsync(
        int pageNumber,
        int pageSize);

    Task<FuelCompositionResponseDto?> GetFuelCompositionByIdAsync(int id);

    Task<FuelCompositionResponseDto> CreateFuelCompositionAsync(CreateFuelCompositionDto dto);

    Task<FuelCompositionResponseDto> UpdateFuelCompositionAsync(
        int id,
        UpdateFuelCompositionDto dto);

    Task DeleteFuelCompositionAsync(int id);
}
