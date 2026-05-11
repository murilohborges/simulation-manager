namespace SimulationManager.Application.DTOs.User;

// DTO for User Creating Request
public record CreateUserDto(
    string UserName,
    string Role
);
