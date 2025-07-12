using CompanyTaskManager.Application;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using CompanyTaskManager.Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure bootstrap logger to capture startup logs before DI container is built
CompanyTaskManager.Web.Extensions.ServiceCollectionExtensions.ConfigureBootstrapLogger();

try
{
    Log.Information("Starting CompanyTaskManager application");
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Configure Serilog with full configuration
    builder.Host.ConfigureSerilog();
    
    // Configure application services
    DataServicesRegistration.AddDataServices(builder.Services, builder.Configuration);
    ApplicationServicesRegistration.AddApplicationServices(builder.Services);
    
    // Configure Identity
    builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
    
    builder.Services.AddControllersWithViews();
    
    var app = builder.Build();
    
    // Configure global exception handling (must be early in pipeline)
    app.UseGlobalExceptionHandling();
    
    // Configure request logging
    app.ConfigureRequestLogging();
    
    // Handle Docker migration requirement
    await HandleDatabaseMigrations(app);
    
    // Configure HTTP request pipeline
    ConfigureHttpPipeline(app);
    
    Log.Information("Application started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

/// <summary>
/// Handles database migrations when running in Docker container.
/// Migrations are triggered by RUN_MIGRATIONS environment variable.
/// </summary>
static async Task HandleDatabaseMigrations(WebApplication app)
{
    var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS");
    
    if (!string.IsNullOrEmpty(runMigrations) && runMigrations.Equals("true", StringComparison.OrdinalIgnoreCase))
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        Log.Information("Running database migrations");
        await db.Database.MigrateAsync();
        Log.Information("Database migrations completed successfully");
    }
}

/// <summary>
/// Configures the HTTP request pipeline based on environment.
/// Sets up error handling, HTTPS redirection, static files, routing, and authentication.
/// </summary>
static void ConfigureHttpPipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts(); // HSTS: 30 days default, consider adjusting for production
    }
    
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
}