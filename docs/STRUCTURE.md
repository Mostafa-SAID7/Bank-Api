# Project Structure

## Overview

This project follows Clean Architecture principles with clear separation of concerns across different layers.

## Directory Structure

```
Bank/
├── bank-backend/
│   ├── Bank.Api/                 # Web API Layer
│   │   ├── Controllers/          # API Controllers
│   │   ├── DTOs/                # Data Transfer Objects
│   │   ├── Middleware/          # Custom Middleware
│   │   └── Program.cs           # Application Entry Point
│   │
│   ├── Bank.Application/        # Application Layer
│   │   ├── Commands/            # CQRS Commands
│   │   ├── Interfaces/          # Service Interfaces
│   │   └── Services/            # Business Logic Services
│   │
│   ├── Bank.Domain/             # Domain Layer
│   │   ├── Entities/            # Domain Entities
│   │   ├── Enums/              # Domain Enumerations
│   │   └── ValueObjects/        # Value Objects
│   │
│   └── Bank.Infrastructure/     # Infrastructure Layer
│       ├── Data/               # Database Context
│       ├── Repositories/       # Data Access Layer
│       └── Services/           # External Services
│
├── docs/                       # Documentation
├── screenshots/                # Project Screenshots
├── .github/                   # GitHub Configuration
└── README.md                  # Project Overview
```

## Layer Responsibilities

### API Layer (Bank.Api)
- HTTP request/response handling
- Authentication and authorization
- Input validation
- API documentation

### Application Layer (Bank.Application)
- Business logic orchestration
- Command and query handling
- Service interfaces
- Application-specific DTOs

### Domain Layer (Bank.Domain)
- Core business entities
- Domain rules and logic
- Value objects
- Domain events

### Infrastructure Layer (Bank.Infrastructure)
- Database access
- External service integrations
- File system operations
- Third-party API clients

## Design Patterns Used

- **Clean Architecture**: Separation of concerns
- **CQRS**: Command Query Responsibility Segregation
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Loose coupling
- **Middleware Pattern**: Request processing pipeline