using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    //we can Register pipeline behaviors here if needed:
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>)); //Validation behaviour for commands, from BuildingBlocks
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>)); // Logging behaviour for commands, from BuildingBlocks
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);

}).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>(); // seed initial data in development for Marten Database
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!); // '! - null-forgiving operator or null suppresion operator' compiler thinks GetConnectionString can return null, but we know it won't

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(options => { }); //configure application to use exception handler
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse, // write health check response in a UI friendly format (JSON)
});

app.Run();
