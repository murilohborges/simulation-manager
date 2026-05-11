using SimulationManager.Application.DTOs.FuelComposition;

namespace SimulationManager.Application.DTOs.User;

// DTO for FuelComposition Response
public record UserReponseDto(
    int Id,
    string UserName,
    string Role,
    DateTime CreatedAt,
    ICollection<FuelCompositionResponseDto>? FuelCompositions = null
);