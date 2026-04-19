using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Interfaces;
using NeverlandEvolved.Infrastructure.Data;
using NeverlandEvolved.Infrastructure.Repositories;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;

namespace NeverlandEvolved.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Databas
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Repositories
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // 3. AutoMapper (Denna stängs nu korrekt direkt)
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<NeverlandEvolved.Application.Mappings.MappingProfile>();
            });

            // 4. FluentValidation
            builder.Services.AddValidatorsFromAssembly(typeof(NeverlandEvolved.Application.Games.Commands.CreateGameCommandValidator).Assembly);

            // 5. MediatR + Pipeline Behaviors (Vikten av ordning: Registrera assembly först, sen behaviors)
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(NeverlandEvolved.Application.Games.Queries.GetAllGamesQuery).Assembly);

                // Detta är "VG-motorn" som kör din validering automatiskt
                cfg.AddOpenBehavior(typeof(NeverlandEvolved.Application.Behaviors.ValidationBehavior<,>));
            });

            // 6. Controllers & JSON
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            // 7. OpenAPI / Swagger
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // --- Middleware-pipelinen (Ordningen här spelar också roll!) ---

            app.UseMiddleware<NeverlandEvolved.API.Middleware.ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}