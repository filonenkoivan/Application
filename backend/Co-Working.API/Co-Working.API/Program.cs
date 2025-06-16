using Co_Woring.Application.Interfaces;
using Co_Woring.Application.Services;
using Co_Working.API.DependencyInjections;
using Co_Working.API.Endpoints;
using Co_Working.API.Middleware;
using Co_Working.Persistence;
using Co_Working.Persistence.Repository;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();


app.MapCoworkingEndpoints();
app.MapWorkspaceEndpoints();
app.MapBookingEndpoints();
app.MapAssistantEndPoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbSeeder.Seed(db);
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("angular");
app.Run();

