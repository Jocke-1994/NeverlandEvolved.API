# NeverlandEvolved API

Ett ASP.NET Core Web API byggt med Clean Architecture, CQRS och JWT-autentisering.

## Teknikstack

- **.NET 10** — ramverk
- **Entity Framework Core 10** — ORM mot SQL Server
- **MediatR** — CQRS-mönster (Commands & Queries)
- **AutoMapper** — mappning mellan entiteter och DTOs
- **FluentValidation** — validering via MediatR Pipeline Behavior
- **JWT Bearer** — autentisering och rollbaserad åtkomstkontroll (RBAC)

---

## Projektstruktur (Clean Architecture)

```
NeverlandEvolved.API/
├── NeverlandEvolved.API/           # Presentationslagret — Controllers, Middleware, Program.cs
├── NeverlandEvolved.Application/   # Applikationslagret — Commands, Queries, DTOs, Validators
├── NeverlandEvolved.Domain/        # Domänlagret — Entiteter, Interface-kontrakt
└── NeverlandEvolved.Infrastructure/# Infrastrukturlagret — EF Core, Repositories, Migrations
```

Varje lager har ett tydligt ansvar och känner bara till lagren innanför sig — `API` känner till `Application`, men aldrig tvärtom.

---

## Kom igång

### Krav
- .NET 10 SDK
- SQL Server eller LocalDB

### Installation

1. Klona repot
2. Öppna `appsettings.Development.json` och kontrollera att anslutningssträngen stämmer
3. Kör migrations för att skapa databasen:
   ```bash
   dotnet ef database update --project NeverlandEvolved.Infrastructure --startup-project NeverlandEvolved.API
   ```
4. Starta API:et:
   ```bash
   dotnet run --project NeverlandEvolved.API
   ```

---

## Endpoints

### Autentisering

| Metod | URL | Beskrivning | Kräver auth |
|-------|-----|-------------|-------------|
| POST | `/api/auth/login` | Logga in och få JWT-token | Nej |

**Request body:**
```json
{
  "username": "admin",
  "password": "dittlösenord"
}
```

---

### Spel (Games)

| Metod | URL | Beskrivning | Kräver auth |
|-------|-----|-------------|-------------|
| GET | `/api/games` | Hämta alla spel | Nej |
| GET | `/api/games/{id}` | Hämta ett spel | Nej |
| POST | `/api/games` | Skapa ett nytt spel | Nej |
| PUT | `/api/games/{id}` | Uppdatera ett spel | Ja (Admin) |
| DELETE | `/api/games/{id}` | Ta bort ett spel | Ja (Admin) |

**Exempel — skapa spel:**
```json
{
  "title": "The Legend of Zelda",
  "genre": "Action-Adventure",
  "price": 499.00
}
```

---

### Användare (Users)

| Metod | URL | Beskrivning | Kräver auth |
|-------|-----|-------------|-------------|
| GET | `/api/users` | Hämta alla användare | Nej |
| GET | `/api/users/{id}` | Hämta en användare | Nej |
| POST | `/api/users` | Skapa en ny användare | Nej |

**Exempel — skapa användare:**
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "hemligt123"
}
```

---

### Recensioner (Reviews)

| Metod | URL | Beskrivning | Kräver auth |
|-------|-----|-------------|-------------|
| POST | `/api/reviews` | Skapa en recension | Nej |

**Exempel — skapa recension:**
```json
{
  "rating": 5,
  "comment": "Fantastiskt spel!",
  "gameId": 1,
  "userId": 1
}
```

---

## Autentisering med JWT

Skyddade endpoints kräver en `Authorization`-header med en giltig JWT-token:

```
Authorization: Bearer <din-token>
```

Logga in via `POST /api/auth/login` för att få en token.

### Roller
- **User** — standardroll för alla nya användare
- **Admin** — krävs för att uppdatera eller ta bort spel

---

## Arkitekturflöde

```
HTTP Request
    ↓
Controller          (tar emot request, skickar Command/Query via MediatR)
    ↓
ValidationBehavior  (FluentValidation körs automatiskt — kastar 400 vid fel)
    ↓
Handler             (utför logiken, anropar Repository)
    ↓
Repository          (kommunicerar med databasen via EF Core)
    ↓
HTTP Response       (Handler returnerar DTO → Controller returnerar 200/201/204)
```
