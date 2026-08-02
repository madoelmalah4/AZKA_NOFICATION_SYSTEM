using Azka.NotificationPlatform.Application;
using Azka.NotificationPlatform.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────────

// Clean Architecture layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register design-time/dev stubs for required application services

// ASP.NET Core
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

// Swagger / OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Azka Notification Platform API",
        Version     = "v1",
        Description = "Enterprise notification dispatch and tracking API for the Azka platform. " +
                      "Supports Email, SMS, and Push channels with full idempotency, " +
                      "template rendering, and delivery audit trail.",
        Contact = new OpenApiContact
        {
            Name  = "Azka Platform Team",
            Email = "platform@azka.internal"
        }
    });

    // Include XML documentation comments from API assembly
    var apiXmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
    if (File.Exists(apiXmlPath))
        options.IncludeXmlComments(apiXmlPath);

    // Include XML documentation comments from Application assembly
    var appXmlFile = "Azka.NotificationPlatform.Application.xml";
    var appXmlPath = Path.Combine(AppContext.BaseDirectory, appXmlFile);
    if (File.Exists(appXmlPath))
        options.IncludeXmlComments(appXmlPath);

    // Display enum members as strings (Email, SMS, Push) instead of integers
    options.UseInlineDefinitionsForEnums();
});

// ── Middleware pipeline ────────────────────────────────────────────────────────

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "Azka Notification Platform API v1");
        ui.RoutePrefix = string.Empty; // Serve Swagger UI at application root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
