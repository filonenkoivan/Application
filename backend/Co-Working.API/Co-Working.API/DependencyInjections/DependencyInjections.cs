using Co_Woring.Application.Interfaces;
using Co_Woring.Application.Services;
using Co_Working.Persistence.Repository;
using Co_Working.Persistence;
using Microsoft.EntityFrameworkCore;
using Co_Working.Persistence.BookingAssistant.Services;

namespace Co_Working.API.DependencyInjections
{
    public static class DependencyInjections
    {
        public static void AddDependencies(this WebApplicationBuilder builder)
        {
            var connectionString = $"Server=postgres;Port=5432;Database=cowork;User Id=postgres;Password=dbpass";
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingServices, BookingService>();
            builder.Services.AddScoped<ICoworkingRepository, CoworkingRepository>();
            builder.Services.AddScoped<ICoworkingService, CoworkingService>();
            builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
            builder.Services.AddScoped<BookingAssistant>();
            builder.Services.AddHttpClient();
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
            builder.Services.AddSwaggerGen();
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5086);
            });
        }
    }
}
