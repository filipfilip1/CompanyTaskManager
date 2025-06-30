namespace CompanyTaskManager.Web.Extensions;

/// <summary>
/// Logging templates for consistent formatting across outputs.
/// </summary>
public static class LoggingTemplates
{
    /// <summary>
    /// Console template for development.
    /// </summary>
    public const string Console = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";
    
    /// <summary>
    /// File template with full timestamp.
    /// </summary>
    public const string File = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";
    
    /// <summary>
    /// HTTP request template with response time.
    /// </summary>
    public const string HttpRequest = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    
    /// <summary>
    /// Log file path with daily rolling.
    /// </summary>
    public const string FilePath = "logs/log-.txt";
}