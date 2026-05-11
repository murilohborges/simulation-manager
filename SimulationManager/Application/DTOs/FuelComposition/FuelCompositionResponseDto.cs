using SimulationManager.Domain.ValueObjects;

namespace SimulationManager.Application.DTOs.FuelComposition;

// DTO for FuelComposition Response
public record FuelCompositionResponseDto(
    int Id,
    string Name,
    FuelCompositionDetails Composition,
    DateTime CreatedAt,
    int UserId
);