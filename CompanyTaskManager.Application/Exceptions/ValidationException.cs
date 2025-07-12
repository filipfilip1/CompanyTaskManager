namespace CompanyTaskManager.Application.Exceptions;

/// <summary>
/// Thrown when business validation rules are violated.
/// </summary>
public class ValidationException : DomainException
{
    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}