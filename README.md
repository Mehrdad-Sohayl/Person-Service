# PersonService

A microservice for managing person information built with **gRPC**, **CQRS**, **MediatR**, and **Clean Architecture** on **.NET 10.0**.

## Overview

PersonService provides CRUD operations for managing people through a gRPC backend with a REST API facade. The solution follows Clean Architecture principles, separating domain logic from infrastructure concerns.

### Key Features

- **Clean Architecture** with proper dependency flow across all layers
- **CQRS** pattern via MediatR for command/query separation
- **Domain-Driven Design** with validated value objects and domain events
- **gRPC** backend for high-performance inter-service communication
- **REST API** facade (BFF pattern) for HTTP clients
- **Soft-delete** with global query filters
- **Optimistic concurrency** via row versioning
- **Docker** support with docker-compose orchestration

## Architecture

```
┌─────────────────────────────────┐
│   PersonService.Client.Api      │
│      (REST API :8080)           │
└──────────────┬──────────────────┘
               │ gRPC
               ▼
┌─────────────────────────────────┐
│      PersonService.Api          │
│     (gRPC Server :5001)         │
└──────────────┬──────────────────┘
               │
               ▼
┌─────────────────────────────────┐
│  PersonService.Application      │
│    (CQRS + MediatR)             │
└──────────────┬──────────────────┘
               │
               ▼
┌─────────────────────────────────┐
│     PersonService.Domain        │
│  (Entities, Value Objects,      │
│   Domain Events)                │
└──────────────┬──────────────────┘
               │
               ▼
┌─────────────────────────────────┐
│  PersonService.Infrastructure   │
│  (EF Core, SQL Server,          │
│   Repositories)                 │
└─────────────────────────────────┘
```

## Project Structure

```
src/
├── PersonService.Api/                  # gRPC server
│   ├── Common/                         # Interceptors, extensions
│   ├── Services/                       # gRPC service implementations
│   └── Program.cs
├── PersonService.Client.Api/           # REST API facade
│   ├── Controllers/                    # API controllers
│   ├── Services/                       # gRPC client services
│   ├── Models/                         # Request/response models
│   └── Program.cs
├── PersonService.Application/          # Business logic layer
│   ├── Commands/                       # CQRS commands
│   ├── Queries/                        # CQRS queries
│   ├── Handlers/                       # Command/query handlers
│   ├── Exceptions/                     # Application exceptions
│   └── Common/                         # Shared types (PagedResult)
├── PersonService.Domain/               # Core domain layer
│   ├── Entities/                       # Domain entities
│   ├── ValueObjects/                   # Validated value objects
│   ├── Events/                         # Domain events
│   ├── Exceptions/                     # Domain exceptions
│   ├── Factories/                      # Entity factories
│   ├── Interfaces/                     # Repository interfaces
│   └── Common/                         # BaseEntity
├── PersonService.Infrastructure/       # Data access layer
│   ├── Data/                           # DbContext
│   ├── Repositories/                   # Repository implementations
│   └── Migrations/                     # EF Core migrations
├── PersonService.Contracts/            # Shared gRPC contracts
│   └── Protos/                         # Protocol Buffer definitions
├── PersonService.Tests/                # Unit tests
│   ├── Domain/                         # Domain layer tests
│   └── Application/                    # Handler tests
├── PersonService.IntegrationTests/     # Integration tests
│   ├── Api/                            # API endpoint tests
│   └── Common/                         # Test infrastructure
├── docker-compose.yml
├── NuGet.config
└── PersonService.sln
```

## Technologies

| Technology | Version | Purpose |
|---|---|---|
| .NET | 10.0-preview | Application platform |
| ASP.NET Core | 10.0-preview | Web hosting |
| gRPC | 2.80 | Service communication |
| MediatR | 14.1.0 | CQRS implementation |
| Entity Framework Core | 9.0.15 | Data access |
| SQL Server | 2022 | Database |
| xUnit | 2.9.3 | Unit testing |
| FluentAssertions | 8.10.0 | Test assertions |
| Moq | 4.20.72 | Test mocking |
| Polly | 8.6.6 | Resilience policies |

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (preview)
- [Docker](https://www.docker.com/products/docker-desktop/) (for containerized setup)
- SQL Server (for local development without Docker)

### Option 1: Docker (Recommended)

```bash
cd src
docker compose up --build
```

This starts:
- **SQL Server** on `localhost:1433`
- **gRPC API** on `localhost:5001`
- **REST API** on `localhost:8080`

### Option 2: Local Development

1. Set up the database connection:

```bash
cd src/PersonService.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=PersonServiceDb;User Id=sa;Password=YourPassword;Encrypt=false;"
```

2. Start SQL Server (or use Docker for just the database):

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

3. Apply database migrations:

```bash
cd src
dotnet ef database update --project PersonService.Infrastructure --startup-project PersonService.Api
```

4. Start the gRPC server:

```bash
cd src/PersonService.Api
dotnet run
```

5. Start the REST API:

```bash
cd src/PersonService.Client.Api
dotnet run
```

## API Endpoints

### REST API (Client.Api)

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/persons` | Create a new person |
| `GET` | `/api/persons/{id}` | Get person by ID |
| `GET` | `/api/persons?pageNumber=1&pageSize=50` | Get paginated list |
| `PUT` | `/api/persons/FirstName` | Update first name |
| `PUT` | `/api/persons/LastName` | Update last name |
| `PUT` | `/api/persons/BirthDate` | Update birth date |
| `DELETE` | `/api/persons/{id}` | Delete a person |

### gRPC Service

| Method | Description |
|---|---|
| `Create` | Create a new person |
| `GetById` | Get person by ID |
| `GetAll` | Get paginated list |
| `UpdateFirstName` | Update first name |
| `UpdateLastName` | Update last name |
| `UpdateBirthDate` | Update birth date |
| `Delete` | Delete a person |

### Example: Create a Person

```bash
curl -X POST http://localhost:8080/api/persons \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "nationalCode": "1234567890",
    "birthDate": "1990-01-15T00:00:00"
  }'
```

## Domain Model

### Person Entity

The core entity with validated value objects:

- **FirstName** (`Name`) — Max 20 characters, non-empty
- **LastName** (`Name`) — Max 20 characters, non-empty
- **NationalCode** (`NationalCode`) — Exactly 10 digits
- **BirthDate** (`BirthDate`) — Cannot be in the future

### Value Objects

Each value object enforces invariants at construction time:

```csharp
var name = new Name("John");           // Valid
var name = new Name("");               // Throws DomainValidationException
var code = new NationalCode("1234567890"); // Valid
var code = new NationalCode("12345");  // Throws DomainValidationException
```

### Domain Events

Events are raised on entity mutations and collected in the base entity:

```csharp
person.UpdateFirstName(new Name("Jane"));
// Raises PersonUpdatedEvent
```

## Configuration

### Environment Variables

| Variable | Description | Default |
|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | `Production` |
| `ConnectionStrings__DefaultConnection` | SQL Server connection string | — |
| `GrpcSettings__PersonServiceUrl` | gRPC server URL | `https://localhost:5001` |
| `GrpcSettings__TimeoutSeconds` | gRPC call timeout | `10` |
| `USE_HTTPS` | Enable HTTPS on gRPC server | `false` |

### Docker Configuration

The `docker-compose.yml` configures:

- SQL Server with health checks
- gRPC API with database connection
- REST API with gRPC client settings
- Volume persistence for database data

## Testing

### Run Unit Tests

```bash
dotnet test PersonService.Tests
```

### Run Integration Tests

```bash
dotnet test PersonService.IntegrationTests
```

### Test Coverage

- **Domain tests**: Value object validation, entity behavior, factory patterns
- **Application tests**: Command/query handler logic, repository mocking
- **Integration tests**: API endpoints with mocked gRPC client via `WebApplicationFactory`

## Development

### Adding Migrations

```bash
dotnet ef migrations add <MigrationName> \
  --project PersonService.Infrastructure \
  --startup-project PersonService.Api
```

### Build

```bash
dotnet build PersonService.sln
```

### Clean Build

```bash
dotnet clean PersonService.sln
dotnet build PersonService.sln
```

## License

This project is provided for educational and demonstration purposes.
