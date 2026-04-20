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
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. DATABAS
// Registrerar AppDbContext med SQL Server som databas-leverantör.
// Anslutningssträngen hämtas från appsettings.json.
// ============================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================================================
// 2. REPOSITORIES
// Kopplar ihop interface (Domain-lagret) med konkreta implementationer
// (Infrastructure-lagret) via Dependency Injection.
// Scoped = en ny instans skapas per HTTP-request.
// ============================================================
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

// ============================================================
// 3. MEDIATR & PIPELINE BEHAVIORS
// MediatR hanterar Commands och Queries (CQRS-mönstret).
// Vi registrerar alla handlers från Application-lagret automatiskt.
// ValidationBehavior körs som ett "mellansteg" innan varje handler,
// och kastar ett fel om FluentValidation-reglerna inte uppfylls.
// ============================================================
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateGameCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// ============================================================
// 4. FLUENTVALIDATION
// Registrerar alla validators från Application-lagret automatiskt.
// Dessa används av ValidationBehavior i MediatR-pipelinen.
// ============================================================
builder.Services.AddValidatorsFromAssembly(typeof(CreateGameCommand).Assembly);

// ============================================================
// 5. AUTOMAPPER
// Registrerar AutoMapper och laddar MappingProfile som definierar
// hur entiteter (t.ex. Game) mappas till DTOs (t.ex. GameDto).
// ============================================================
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// ============================================================
// 6. JWT AUTHENTICATION
// Hämtar JWT-inställningar från appsettings.json och konfigurerar
// Bearer-autentisering. Varje inkommande request med en giltig
// JWT-token autentiseras automatiskt.
// ============================================================
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
        ValidateIssuerSigningKey = true,           // Kontrollera att signaturen stämmer
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,                     // Kontrollera att rätt server utfärdade token
        ValidateAudience = true,                   // Kontrollera att token är avsedd för denna app
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero                  // Ingen tolerans för utgångna tokens
    };
});

// Aktiverar [Authorize]-attributet i controllers
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// ============================================================
// 7. SCALAR (API-dokumentation)
// Genererar ett interaktivt UI där man kan testa alla endpoints.
// OpenAPI-specifikationen skapas automatiskt av ASP.NET Core.
// ============================================================
builder.Services.AddOpenApi();

var app = builder.Build();

// ============================================================
// MIDDLEWARE PIPELINE
// Ordningen här är viktig! ExceptionHandling måste ligga först
// så att alla fel som uppstår längre ner i pipelinen fångas upp.
// ============================================================
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Scalar körs bara i utvecklingsläge — aldrig i produktion
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// UseAuthentication måste alltid komma FÖRE UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
