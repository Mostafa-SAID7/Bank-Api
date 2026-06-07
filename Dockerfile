# Multi-stage build for Bank API
# This Dockerfile is used by Railpack for containerized deployment

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install curl for health checks and bash for startup script
RUN apt-get update && apt-get install -y curl bash && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/Bank.Api/Bank.Api.csproj", "Bank.Api/"]
COPY ["src/Bank.Application/Bank.Application.csproj", "Bank.Application/"]
COPY ["src/Bank.Domain/Bank.Domain.csproj", "Bank.Domain/"]
COPY ["src/Bank.Infrastructure/Bank.Infrastructure.csproj", "Bank.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Bank.Api/Bank.Api.csproj"

# Copy source code
COPY src/ .

# Build application
WORKDIR "/src/Bank.Api"
RUN dotnet build "Bank.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Copy startup script
COPY start.sh /app/start.sh
RUN chmod +x /app/start.sh

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Health check endpoint
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

# Use the startup script to handle environment variables dynamically
ENTRYPOINT ["/app/start.sh"]
