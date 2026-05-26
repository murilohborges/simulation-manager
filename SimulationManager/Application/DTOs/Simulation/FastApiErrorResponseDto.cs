namespace SimulationManager.Application.DTOs.Simulation;

public record FastApiErrorResponseDto
{
    public required string Error { get; init; }
    public required string Type { get; init; }

    // It's nullable for some cases
    public List<FastApiErrorDetailDto>? Details { get; init; } = new();
}

public record FastApiErrorDetailDto
{
    public List<string> Loc { get; init; } = new();
    public required string Msg { get; init; }
    public required string Type { get; init; }
}
