using SimulationManager.Domain.ValueObjects;

namespace SimulationManager.Application.DTOs.Simulation;

public record SimulationResponseDto(
    int Id,
    string Status,
    int UserId,
    int FuelCompositionId,
    SimulationParametersDetails Parameters,
    DateTime CreatedAt,
    SimulationResultResponseDto? Result
);
