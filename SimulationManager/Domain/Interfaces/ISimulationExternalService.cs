using SimulationManager.Application.DTOs.Simulation;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.ValueObjects;

namespace SimulationManager.Domain.Interfaces;

public interface ISimulationExternalService
{
    Task<FastApiResponseDto> ExecuteAsync(
        FuelCompositionDetails composition,
        SimulationParametersDetails parameters);
}
