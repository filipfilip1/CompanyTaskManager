namespace CompanyTaskManager.Application.Exceptions;

/// <summary>
/// Thrown when a requested resource cannot be found.
/// </summary>
public class NotFoundException : DomainException
{
    public NotFoundException(string resourceName, object key) 
        : base($"{resourceName} with identifier '{key}' was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}