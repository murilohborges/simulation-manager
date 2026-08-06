using SimulationManager.Application.DTOs.Common;
using SimulationManager.Application.DTOs.Simulation;

namespace SimulationManager.Application.Interfaces;

public interface ISimulationService
{
    Task<PagedResponseDto<SimulationResponseDto>> GetPagedSimulationsAsync(
        int pageNumber,
        int pageSize);

    Task<SimulationResponseDto?> GetSimulationByIdAsync(int id);

    Task<SimulationResponseDto> CreateSimulationAsync(CreateSimulationDto dto);

    Task DeleteSimulationAsync(int id);
}
