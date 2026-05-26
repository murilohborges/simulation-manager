using Microsoft.EntityFrameworkCore;
using SimulationManager.Infrastructure.Persistence;
using SimulationManager.Infrastructure.Persistence.Repositories;
using System.Text.Json.Serialization;
using SimulationManager.Infrastructure.Seed;
using SimulationManager.Domain.Interfaces;
using SimulationManager.Api.Middlewares;
using SimulationManager.Infrastructure.ExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

string ?mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFuelCompositionRepository, FuelCompositionRepository>();
builder.Services.AddScoped<ISimulationRepository, SimulationRepository>();

// External Service
builder.Services.AddHttpClient<ISimulationExternalService, SimulationExternalService>(client =>
{
    client.BaseAddress = new Uri("https://cycle-comb-calc.onrender.com/");
    client.Timeout = TimeSpan.FromSeconds(60);
});

var app = builder.Build();

// Add middleware extension
app.UseGlobalExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
      options.SwaggerEndpoint("/openapi/v1.json", "simulation manager api"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // guarantees that the bank exists
    await context.Database.MigrateAsync();

    await DatabaseSeeder.SeedAsync(context);
}

app.Run();
