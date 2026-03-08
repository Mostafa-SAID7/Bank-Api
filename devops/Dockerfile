# Multi-stage build for Bank API
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["bank-backend/Bank.Api/Bank.Api.csproj", "Bank.Api/"]
COPY ["bank-backend/Bank.Application/Bank.Application.csproj", "Bank.Application/"]
COPY ["bank-backend/Bank.Domain/Bank.Domain.csproj", "Bank.Domain/"]
COPY ["bank-backend/Bank.Infrastructure/Bank.Infrastructure.csproj", "Bank.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Bank.Api/Bank.Api.csproj"

# Copy source code
COPY bank-backend/ .

# Build application
WORKDIR "/src/Bank.Api"
RUN dotnet build "Bank.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Bank.Api.dll"]