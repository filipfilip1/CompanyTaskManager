namespace CompanyTaskManager.Application.Exceptions;

/// <summary>
/// Thrown when a user attempts to access resources they don't have permission for.
/// </summary>
public class UnauthorizedAccessException : DomainException
{
    public UnauthorizedAccessException(string message) : base(message)
    {
    }

    public UnauthorizedAccessException(string resource, string action) 
        : base($"Access denied. You don't have permission to {action} {resource}.")
    {
    }
}