NeverlandEvolved - Web API
Detta projekt är utvecklat som en del av utbildningen till systemutvecklare (.NET). Applikationen är byggd med fokus på Clean Architecture, säkerhet och tydlig ansvarsfördelning.

🏗 Arkitektur
Projektet är uppdelat i fyra lager enligt Clean Architecture-principen för att säkerställa testbarhet och skalbarhet:

Domain: Innehåller entiteter och gränssnitt.

Application: Innehåller affärslogik, DTOs, Mappings och CQRS-mönstret (MediatR).

Infrastructure: Hanterar databaskontext (EF Core) och implementation av repositories.

API: Utgör applikationens ingångspunkt med controllers och middleware.

✨ Nyckelfunktioner
CQRS med MediatR: Separerar läs- och skrivoperationer för en renare kodbas.

Validation Pipeline: Automatisk validering av inkommande data med FluentValidation via MediatR-behaviors.

Säkerhet: Implementation av JWT (JSON Web Token) för autentisering.

RBAC (Role-Based Access Control): Behörighetsstyrning baserat på roller (Admin/User).

Global Exception Handling: Custom middleware som fångar upp fel och returnerar standardiserade svar (t.ex. vid valideringsfel).

AutoMapper: Hanterar mappning mellan entiteter och DTOs för att skydda domänmodellen.

🛠 Tech Stack
Runtime: .NET 10 (Preview)

Databas: SQL Server (LocalDB) via Entity Framework Core

Verktyg: MediatR, AutoMapper, FluentValidation, Swashbuckle (OpenAPI/Scalar)

📝 Kommentar angående AutoMapper
I den nuvarande versionen är registreringen av AutoMapper i Program.cs kommenterad på grund av en versionskonflikt mellan paket (CS0433). All mappningslogik och profiler finns dock implementerade i Application-lagret. Detta gjordes för att säkerställa att projektet bygger korrekt och är körbart för rättning.
