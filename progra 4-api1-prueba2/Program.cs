using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Servicios mínimos
builder.Services.AddControllers();



var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



// Puedes exponer rutas separadas para readiness/liveness si lo prefieres
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = entry => true // filtra checks que consideres para readiness
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = entry => false // liveness mínima; devuelve 200 si el host corre
});

app.Run();
