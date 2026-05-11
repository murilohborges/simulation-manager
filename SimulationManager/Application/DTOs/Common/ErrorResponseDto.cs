namespace SimulationManager.Application.DTOs.Common;

// DTO for Error exception response's
public class ErrorResponseDto
{
    public int StatusCode { get; init; }
    public string Message { get; init; }
    public DateTime Timestamp { get; init; }

    public ErrorResponseDto(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}
