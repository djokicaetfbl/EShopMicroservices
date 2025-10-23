
using BuildingBlocks.Exceptions.Handler;

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

builder.Services.AddCarter(); // to use minimal API with Carter

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName); // register ShoppingCart document
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter(); // Map Carter endpoints to our API project
app.UseExceptionHandler(options => { });

app.Run();
