# Project Setup Guide

## Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server or SQL Server Express
- Git

## Installation Steps

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/bank-management-system.git
cd bank-management-system
```

### 2. Database Setup

1. Install SQL Server or SQL Server Express
2. Create a new database named `BankDB`
3. Update connection string in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BankDB;Trusted_Connection=true;"
  }
}
```

### 3. Build the Solution

```bash
cd bank-backend
dotnet restore
dotnet build
```

### 4. Run Database Migrations

```bash
dotnet ef database update --project Bank.Infrastructure --startup-project Bank.Api
```

### 5. Run the Application

```bash
cd Bank.Api
dotnet run
```

The API will be available at `https://localhost:7000`

## Development Environment

### Recommended Tools

- **IDE**: Visual Studio 2022 or VS Code
- **Database**: SQL Server Management Studio
- **API Testing**: Postman or Swagger UI
- **Version Control**: Git

### Environment Variables

Create a `.env` file in the root directory:

```
DATABASE_CONNECTION_STRING=your_connection_string
JWT_SECRET=your_jwt_secret_key
API_PORT=7000
```

## Testing

### Run Unit Tests

```bash
dotnet test
```

### Run Integration Tests

```bash
dotnet test --filter Category=Integration
```

## Troubleshooting

### Common Issues

1. **Database Connection Failed**
   - Verify SQL Server is running
   - Check connection string format
   - Ensure database exists

2. **Build Errors**
   - Run `dotnet clean` then `dotnet restore`
   - Check .NET SDK version compatibility

3. **Port Already in Use**
   - Change port in `launchSettings.json`
   - Kill existing processes using the port

### Getting Help

- Check the [Issues](https://github.com/yourusername/bank-management-system/issues) page
- Review the [Contributing Guide](CONTRIBUTING.md)
- Contact the development team