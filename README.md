# 👤 PersonService

A sample microservice for managing person information using **gRPC**, **CQRS**, **MediatR**, and **Clean Architecture** in **.NET 6**.

---

## 🚀 Overview

**PersonService** is a microservice that provides CRUD operations for managing people through gRPC endpoints.

The solution follows **Clean Architecture** principles and separates responsibilities into dedicated layers:

* 🎯 **Domain** – Entities, Value Objects, Domain Events
* ⚙️ **Application** – Commands, Queries, Handlers, Business Use Cases
* 🗄️ **Infrastructure** – EF Core, SQL Server, Repositories, Migrations
* 🌐 **API** – gRPC Server
* 🔄 **Client API** – REST-to-gRPC Adapter
* 📜 **Contracts** – Shared gRPC contracts and `.proto` definitions

---

## 🏛️ Architecture

```text
┌──────────────────────────┐
│ PersonService.Client.Api │
│      (REST API)          │
└────────────┬─────────────┘
             │
             │ gRPC
             ▼
┌──────────────────────────┐
│     PersonService.Api    │
│      (gRPC Server)       │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│ PersonService.Application│
│      (CQRS + MediatR)    │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│   PersonService.Domain   │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│PersonService.Infrastructure│
│    (EF Core + SQL Server) │
└──────────────────────────┘
```

---

## 📂 Solution Structure

```text
src/
├── PersonService.Api
├── PersonService.Client.Api
├── PersonService.Application
├── PersonService.Domain
├── PersonService.Infrastructure
└── PersonService.Contracts

tests/
├── PersonService.UnitTests
└── PersonService.IntegrationTests
```

---

## 🧩 Domain Layer

The Domain layer contains the core business model of the application.

### Features

* ✅ Entities
* ✅ Value Objects
* ✅ Domain Events
* ✅ Base Entity abstraction

Domain Events are collected within the base entity and can be raised by domain objects when state changes occur.

Example concepts:

* Person
* NationalCode
* Domain Events related to Person lifecycle operations

---

## ⚡ Application Layer

The Application layer implements the **CQRS** pattern using **MediatR**.

### Commands

* CreatePersonCommand
* UpdatePersonCommand
* DeletePersonCommand

### Queries

* GetPersonByIdQuery
* GetAllPersonsQuery

### Handlers

Each command and query is processed through a dedicated handler.

```text
Controller / gRPC Service
        ↓
     MediatR
        ↓
 Handler
        ↓
 Repository
```

---

## 🗄️ Infrastructure Layer

Infrastructure provides persistence and external integrations.

### Technologies

* Entity Framework Core
* SQL Server
* Repository Pattern
* EF Core Migrations

### Repository Separation

The project follows a read/write repository separation:

```text
Read Repository
Write Repository
```

This aligns with the CQRS architecture used in the Application layer.

---

## 🌐 gRPC API

The main service is exposed through gRPC.

Protocol definitions are maintained in:

```text
PersonService.Contracts
```

using:

```text
person.proto
```

### Supported Operations

| Operation     | Description                |
| ------------- | -------------------------- |
| CreatePerson  | Creates a new person       |
| GetPersonById | Retrieves a person by ID   |
| GetAllPersons | Retrieves all persons      |
| UpdatePerson  | Updates an existing person |
| DeletePerson  | Deletes a person           |

---

## 🔄 Client API

The solution contains a separate REST API project that acts as a bridge between REST clients and the gRPC service.

```text
REST Client
     ↓
PersonService.Client.Api
     ↓
gRPC
     ↓
PersonService.Api
```

This allows consumers that do not support gRPC to interact with the system through standard HTTP APIs.

---

## 🧪 Testing

The solution includes automated tests written with **xUnit**.

### Unit Tests

Focus on:

* Domain behavior
* Application handlers
* Business rules

### Integration Tests

Focus on:

* API behavior
* End-to-end request flow
* Database interactions

* **End-to-End flow is under construction!**

FluentValidation is used within integration test scenarios to validate request expectations.

---

## ⚙️ Configuration

The application uses standard `.NET` configuration files.

### API Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  }
}
```

### Default gRPC Endpoint

```text
https://localhost:5001
```

Configuration typically includes:

* Database connection string
* Logging settings
* gRPC endpoint settings

---

## 🛠️ Build & Run

### Restore Packages

```bash
dotnet restore
```

### Build Solution

```bash
dotnet build
```

### Run gRPC Server

```bash
cd src/PersonService.Api
dotnet run
```

### Run REST Client API

```bash
cd src/PersonService.Client.Api
dotnet run
```

---

## 📚 Technologies

| Technology            | Purpose                     |
| --------------------- | --------------------------- |
| .NET 9                | Application Platform        |
| ASP.NET Core          | Hosting                     |
| gRPC                  | Service Communication       |
| MediatR               | CQRS Implementation         |
| Entity Framework Core | Data Access                 |
| SQL Server            | Database                    |
| xUnit                 | Testing                     |
| FluentValidation      | Validation (Test Scenarios) |

---

## 📌 Notes

* The solution follows Clean Architecture principles.
* CQRS is implemented using MediatR.
* Domain Events are supported and stored within entities.
* gRPC contracts are maintained separately in the Contracts project.
* Database schema changes are managed through EF Core migrations.

---

## 📄 License

This project is provided for educational and demonstration purposes.
