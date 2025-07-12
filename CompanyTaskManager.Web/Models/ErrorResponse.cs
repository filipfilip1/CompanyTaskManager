namespace CompanyTaskManager.Web.Models;

/// <summary>
/// Standardized error response model for consistent API error handling.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Human-readable error message safe for display to users.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// HTTP status code for the error.
    /// </summary>
    public int StatusCode { get; set; }
    
    /// <summary>
    /// Unique identifier for this error occurrence for tracking purposes.
    /// </summary>
    public string TraceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Additional error details (only included in Development environment).
    /// </summary>
    public object? Details { get; set; }
}