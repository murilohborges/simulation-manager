using SimulationManager.Domain.ValueObjects;

namespace SimulationManager.Application.DTOs.FuelComposition;

// DTO for FuelComposition Creating Request
public record CreateFuelCompositionDto(
    string Name,
    FuelCompositionDetails Composition,
    int UserId
);