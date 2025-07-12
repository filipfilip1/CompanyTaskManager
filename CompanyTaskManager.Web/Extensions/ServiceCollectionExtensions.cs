using CompanyTaskManager.Web.Middleware;
using Serilog;
using Serilog.Events;

namespace CompanyTaskManager.Web.Extensions;

/// <summary>
/// Extension methods for configuring application services and middleware.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures bootstrap logger to capture startup logs before DI container is built.
    /// </summary>
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }
    
    /// <summary>
    /// Configures Serilog with application-specific settings from configuration.
    /// </summary>
    public static IHostBuilder ConfigureSerilog(this IHostBuilder host)
    {
        return host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .ConfigureEnrichers()
            .ConfigureWriters(context.HostingEnvironment));
    }
    
    /// <summary>
    /// Configures request logging with user context enrichment.
    /// </summary>
    public static WebApplication ConfigureRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = LoggingTemplates.HttpRequest;
            
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
                
                if (httpContext.User.Identity?.IsAuthenticated == true)
                {
                    diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
                }
            };
        });
        
        return app;
    }
    
    /// <summary>
    /// Adds application name and log context to all entries.
    /// </summary>
    private static LoggerConfiguration ConfigureEnrichers(this LoggerConfiguration configuration)
    {
        return configuration
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "CompanyTaskManager");
    }
    
    /// <summary>
    /// Configures console and file output with 7-day retention.
    /// </summary>
    private static LoggerConfiguration ConfigureWriters(this LoggerConfiguration configuration, IHostEnvironment environment)
    {
        return configuration
            .WriteTo.Console(outputTemplate: LoggingTemplates.Console)
            .WriteTo.File(
                path: LoggingTemplates.FilePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: LoggingTemplates.File);
    }
    
    /// <summary>
    /// Configures global exception handling middleware.
    /// </summary>
    public static WebApplication UseGlobalExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        return app;
    }
}