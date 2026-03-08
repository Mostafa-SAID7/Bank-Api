# Deployment Guide

## Deployment Options

### Local Development
- IIS Express (Visual Studio)
- Kestrel (dotnet run)
- Docker containers

### Production Environments
- Azure App Service
- AWS Elastic Beanstalk
- Docker containers
- On-premises IIS

## Docker Deployment

### Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Bank.Api/Bank.Api.csproj", "Bank.Api/"]
COPY ["Bank.Application/Bank.Application.csproj", "Bank.Application/"]
COPY ["Bank.Domain/Bank.Domain.csproj", "Bank.Domain/"]
COPY ["Bank.Infrastructure/Bank.Infrastructure.csproj", "Bank.Infrastructure/"]

RUN dotnet restore "Bank.Api/Bank.Api.csproj"
COPY . .
WORKDIR "/src/Bank.Api"
RUN dotnet build "Bank.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank.Api.dll"]
```

### Docker Compose
```yaml
version: '3.8'

services:
  bank-api:
    build: .
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=BankDB;User=sa;Password=YourPassword123;
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

volumes:
  sqldata:
```

## Azure Deployment

### Azure App Service

1. **Create App Service**
```bash
az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name myBankApp --runtime "DOTNET|9.0"
```

2. **Configure Connection String**
```bash
az webapp config connection-string set --resource-group myResourceGroup --name myBankApp --settings DefaultConnection="Server=myserver.database.windows.net;Database=BankDB;User ID=myuser;Password=mypassword;" --connection-string-type SQLAzure
```

3. **Deploy Application**
```bash
dotnet publish -c Release
az webapp deployment source config-zip --resource-group myResourceGroup --name myBankApp --src publish.zip
```

### Azure SQL Database

1. **Create SQL Server**
```bash
az sql server create --name myserver --resource-group myResourceGroup --location "East US" --admin-user myadmin --admin-password mypassword
```

2. **Create Database**
```bash
az sql db create --resource-group myResourceGroup --server myserver --name BankDB --service-objective Basic
```

## Environment Configuration

### Production Settings

#### appsettings.Production.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "#{ConnectionString}#"
  },
  "JwtSettings": {
    "SecretKey": "#{JwtSecretKey}#",
    "Issuer": "BankAPI",
    "Audience": "BankClients",
    "ExpirationMinutes": 60
  }
}
```

### Environment Variables
```bash
# Database
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=BankDB;..."

# JWT
export JwtSettings__SecretKey="your-super-secret-key"

# Logging
export ASPNETCORE_ENVIRONMENT="Production"
```

## CI/CD Pipeline

### GitHub Actions
```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 9.0.x
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --no-restore
      
    - name: Test
      run: dotnet test --no-build --verbosity normal
      
    - name: Publish
      run: dotnet publish -c Release -o ./publish
      
    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'myBankApp'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: './publish'
```

## Monitoring and Logging

### Application Insights
```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

### Health Checks
```csharp
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString)
    .AddCheck("self", () => HealthCheckResult.Healthy());

app.MapHealthChecks("/health");
```

## Security Considerations

- Use HTTPS in production
- Implement proper authentication
- Configure CORS appropriately
- Use secure connection strings
- Enable request logging
- Implement rate limiting
- Regular security updates