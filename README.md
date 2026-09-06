# VenueReservation

A **.NET 10** Web API for booking conference halls / event venues, built with **Clean Architecture**, **CQRS (MediatR)**, and **PostgreSQL**. It lets clients search for available venues, book them for a time slot with optional add-on services (projector, Wi-Fi, sound, etc.), and pull back revenue and utilization analytics.

---

## Table of Contents

- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Domain Model & Business Rules](#domain-model--business-rules)
- [API Reference](#api-reference)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database & Migrations](#database--migrations)
- [Error Handling](#error-handling)
- [Roadmap / Known Gaps](#roadmap--known-gaps)

---

## Architecture

The solution follows **Clean Architecture** with a strict dependency direction (outer layers depend on inner layers, never the reverse), combined with a **CQRS** pattern implemented via **MediatR**:

```
VenueReservation.Domain          → Entities, value objects, domain errors — no external dependencies
VenueReservation.Application     → Commands/Queries, handlers, validators, repository interfaces
VenueReservation.Infrastructure  → EF Core DbContext, repositories, migrations, DI wiring
VenueReservation (Api)           → Controllers, request/response DTOs, mapping, composition root
```

Requests flow as: **Controller → MediatR Command/Query → Handler → Domain → Repository (EF Core) → PostgreSQL**, with **FluentValidation** running as a MediatR pipeline behavior before any handler executes, and a `Result` / `Result<T>` object propagating success/failure without throwing exceptions for expected business-rule violations.

## Tech Stack

| Concern | Technology |
|---|---|
| Runtime | .NET 10 / ASP.NET Core Web API |
| Database | PostgreSQL (via `Npgsql.EntityFrameworkCore.PostgreSQL`) |
| ORM | Entity Framework Core 10 |
| CQRS / Mediator | MediatR 14 |
| Validation | FluentValidation 12 (as a MediatR pipeline behavior) |
| Object Mapping | Mapster |
| API Docs | Swashbuckle (Swagger / OpenAPI) — available in the `Development` environment |
| Error Handling | `IExceptionHandler` (`GlobalExceptionHandler`) + RFC 7807 `ProblemDetails` |

## Project Structure

```
VenueReservation.Domain/
├── Models/
│   ├── Venues/            Venue aggregate root + ValueObjects (VenueName, Capacity, PricePerHour)
│   ├── Reservations/      Reservation aggregate + ValueObjects (BookingPeriod, BookingPrice)
│   ├── Services/          Service entity + ValueObjects (ServiceName, ServicePrice)
│   └── ReservationService.cs   Join entity for the Reservation ↔ Service many-to-many
└── Results/                Result / Result<T> + typed DomainError hierarchy per aggregate

VenueReservation.Application/
├── Features/
│   ├── Venues/             Commands (Create/Update/Delete) & Queries (Search, GetById, Revenue, Utilization)
│   └── Reservations/       BookVenue command
├── Interfaces/              IVenueRepository, IReservationRepository
└── Validation/              ValidationBehavior<TRequest,TResponse> pipeline + ValidationError / NotFoundError

VenueReservation.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Repositories/        VenueRepository, ReservationRepository
│   └── Seeders/              DatabaseSeeder (seeds 3 demo venues with default services)
├── Configurations/           EF Core Fluent API entity configurations
├── Migrations/                EF Core migrations
└── Extensions/                DependencyInjection.cs — AddInfrastructure() / InitializeDatabaseAsync()

VenueReservation/ (Api)
├── Controllers/               VenuesController, ReservationsController, AnalyticsController
├── DTOs/                       Request contracts per feature
├── Mappings/                   Mapster mapping configs
├── GlobalExceptionHandler.cs
└── Program.cs                  Composition root
```

## Domain Model & Business Rules

**Venue** — aggregate root with a name, capacity, base hourly price, and a collection of `Service` add-ons it can offer. Supports create/update/soft-delete, and enforces uniqueness of service names within a venue.

**Reservation** — created from a `Venue`, a `BookingPeriod`, and a set of selected services; the venue must actually offer every selected service or creation fails. Total price is calculated from the venue's rental cost over the period plus the sum of selected services' prices.

**Service** — an optional add-on (e.g. projector, Wi-Fi) tied to a specific venue, with its own name and price.

Key invariants enforced by value objects:

- **Capacity** — must be between **10 and 500**.
- **BookingPeriod**:
  - Start time must fall on the hour (no minutes/seconds).
  - Bookings must start at least ~50 minutes in the future.
  - Duration must be between **1 and 12 hours**.
  - The entire booking must fall within working hours (**06:00–23:00**).
  - Rent cost applies a time-of-day multiplier: **+15%** for the 12:00–14:00 peak window, **-10%** for the early 06:00–09:00 window, **-20%** for the 18:00–23:00 evening window, and the base rate otherwise.

All construction and mutation goes through static factory methods (`Create`, `Update`) that return a `Result` / `Result<T>` instead of throwing, so invalid states are impossible to represent and validation failures are handled uniformly.

## API Reference

All endpoints are under `/api`. Interactive Swagger UI is available at `/swagger` when running in the `Development` environment.

### Venues — `/api/venues`

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/venues` | Create a venue (with optional initial services) |
| `PUT` | `/api/venues/{id}` | Update a venue's details and service list |
| `DELETE` | `/api/venues/{id}?isSoftDelete=true` | Delete a venue (soft-delete by default) |
| `GET` | `/api/venues/search?dateTime=&durationInHours=&capacity=` | Find venues available for a given time window and capacity |
| `GET` | `/api/venues/{id}` | Get a single venue by id |

### Reservations — `/api/reservations`

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/reservations` | Book a venue for a time slot with optional services |

### Analytics — `/api/analytics`

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/analytics/revenue?from=&to=` | Revenue report per venue for a date range |
| `GET` | `/api/analytics/utilization?from=&to=` | Utilization report per venue for a date range |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A running PostgreSQL instance

### Run locally

```bash
git clone <repository-url>
cd VenueReservation.Api

# Point the connection string at your own PostgreSQL instance (see Configuration below),
# then restore and run the API project:
dotnet restore
dotnet run --project VenueReservation.Api
```

On startup, the API automatically applies pending EF Core migrations and seeds three demo venues (with default services) if the database is empty — no manual setup required beyond having PostgreSQL reachable.

Once running, open `/swagger` to explore and try the endpoints.

## Configuration

Connection settings live in `VenueReservation.Api/appsettings.json` under `ConnectionStrings:PostgresConnection`.

```json
{
  "ConnectionStrings": {
    "PostgresConnection": "Host=localhost;Port=5432;Database=VenueReservationDb;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

## Database & Migrations

The `Infrastructure` project owns the EF Core `ApplicationDbContext`, entity configurations, and migrations. Migrations are applied automatically at application startup via `InitializeDatabaseAsync()`.

To add a new migration manually:

```bash
dotnet ef migrations add <MigrationName> \
  --project VenueReservation.Infrastructure \
  --startup-project VenueReservation.Api
```

## Error Handling

Domain and validation failures never throw; they flow back as a `Result` whose `DomainError` is translated by `ApiControllerBase.Problem()` into an RFC 7807-style `ProblemDetails` response:

| Error type | HTTP status |
|---|---|
| Validation | `400 Bad Request` |
| Not Found | `404 Not Found` |
| Conflict (business rule violation) | `409 Conflict` |
| Unhandled | `500 Internal Server Error` |

Unhandled exceptions are additionally caught globally by `GlobalExceptionHandler`.

## Roadmap / Known Gaps

- `CreateServiceRequest` / `UpdateServiceRequest` DTOs exist but there are currently no standalone `Services` endpoints — services are only managed inline through venue create/update.
- No authentication/authorization is currently wired up.
- No automated test project is present in the solution yet.
