using Co_Woring.Application.Interfaces;
using Co_Woring.Application.Services;
using Co_Working.API.Endpoints;
using Co_Working.API.Middleware;
using Co_Working.Persistence;
using Co_Working.Persistence.Repository;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = $"Server=localhost;Port=5432;Database=cowork;User Id=postgres;Password=dbpass";


builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingServices, BookingService>();
builder.Services.AddCors(opt => opt.AddPolicy("angular", policy =>
{
    policy.WithOrigins("http://localhost:4200");
    policy.AllowAnyMethod();
    policy.AllowCredentials();
    policy.AllowAnyHeader();
}));
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(connectionString)
);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddSwaggerGen();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5086);
});

var app = builder.Build();

app.MapBookingEndpoints();
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

