using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Interfaces;
using NeverlandEvolved.Infrastructure.Data;
using NeverlandEvolved.Infrastructure.Repositories;
using Scalar.AspNetCore; // Importera din DbContext
using System.Text.Json.Serialization;
using AutoMapper;


namespace NeverlandEvolved.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(NeverlandEvolved.Application.Games.Queries.GetAllGamesQuery).Assembly));
            // Vi skickar in själva klassen direkt, AutoMapper fattar då vilket projekt den ska leta i
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<NeverlandEvolved.Application.Mappings.MappingProfile>();
            });

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
