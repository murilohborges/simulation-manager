using SimulationManager.Domain.ValueObjects;

namespace SimulationManager.Application.DTOs.FuelComposition;

public record UpdateFuelCompositionDto
(
    string Name,
    FuelCompositionDetails Composition
);
