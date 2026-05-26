using SimulationManager.Domain.ValueObjects;

namespace SimulationManager.Application.DTOs.Simulation;

public record CreateSimulationDto(
    SimulationParametersDetails Parameters, 
    int FuelCompositionId,
    int UserId
);
