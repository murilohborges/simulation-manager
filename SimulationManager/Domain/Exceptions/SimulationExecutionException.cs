namespace SimulationManager.Domain.Exceptions;

public class SimulationExecutionException : Exception
{
    public string ErrorType { get; }
    public List<ValidationErrorDetail> ValidationErrors { get; }

    public SimulationExecutionException(
        string message,
        string errorType,
        List<ValidationErrorDetail>? validationErrors = null)
        : base(message)
    {
        ErrorType = errorType;
        ValidationErrors = validationErrors ?? new();
    }
}

public record ValidationErrorDetail(
    string Field,
    string Message,
    string Type
);