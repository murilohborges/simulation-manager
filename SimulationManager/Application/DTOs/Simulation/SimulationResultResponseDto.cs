namespace SimulationManager.Application.DTOs.Simulation;

public record SimulationResultResponseDto (
    int Id,
    FastApiResponseDto Result,
    DateTime CreatedAt
);
