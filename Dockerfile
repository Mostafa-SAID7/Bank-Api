# Multi-stage build for Bank API
# Simplified for Railpack deployment compatibility

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY src/Bank.Api/Bank.Api.csproj Bank.Api/
COPY src/Bank.Application/Bank.Application.csproj Bank.Application/
COPY src/Bank.Domain/Bank.Domain.csproj Bank.Domain/
COPY src/Bank.Infrastructure/Bank.Infrastructure.csproj Bank.Infrastructure/

# Copy NuGet config
COPY NuGet.Config .

# Restore dependencies
RUN dotnet restore Bank.Api/Bank.Api.csproj

# Copy all source code
COPY src/ .

# Build
WORKDIR /src/Bank.Api
RUN dotnet build Bank.Api.csproj -c Release

# Stage 2: Publish
FROM build AS publish
WORKDIR /src/Bank.Api
RUN dotnet publish Bank.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install dependencies for startup script
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Copy startup script
COPY start.sh .
RUN chmod +x start.sh

# Expose port
EXPOSE 80
EXPOSE 443

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:5000/health || exit 1

# Run application
ENTRYPOINT ["./start.sh"]
