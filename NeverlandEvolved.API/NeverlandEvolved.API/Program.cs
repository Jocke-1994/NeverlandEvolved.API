using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MediatR;
using NeverlandEvolved.API.Middleware;
using NeverlandEvolved.Application.Behaviors;
using NeverlandEvolved.Application.Games.Commands;
using NeverlandEvolved.Application.Mappings;
using NeverlandEvolved.Domain.Interfaces;
using NeverlandEvolved.Infrastructure.Data;
using NeverlandEvolved.Infrastructure.Repositories;

// OBS: Om Scalar/Swagger bråkar, låter vi dessa usings vara borttagna/kommenterade
// using Microsoft.OpenApi.Models; 
// using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. DATABAS
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. REPOSITORIES
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 3. MEDIATR & PIPELINE BEHAVIORS
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateGameCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// 4. FLUENTVALIDATION & AUTOMAPPER
builder.Services.AddValidatorsFromAssembly(typeof(CreateGameCommand).Assembly);

// FIX: Detta är den mest stabila raden för AutoMapper
//Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions.AddAutoMapper(builder.Services, typeof(MappingProfile));

// 5. JWT AUTHENTICATION
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// --- MIDDLEWARE PIPELINE ---
app.UseMiddleware<ExceptionHandlingMiddleware>();

// NÖDPLAN: Vi kommenterar bort Swagger/Scalar-anropen här också 
// så att appen inte kraschar när den startar.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => { options.RouteTemplate = "openapi/{documentName}.json"; });
    app.MapScalarApiReference();
}
*/

app.UseHttpsRedirection();

// VIKTIG ORDNING
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();