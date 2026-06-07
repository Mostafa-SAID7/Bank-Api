#!/bin/bash

# Bank API startup script for Railpack deployment
# Automatically detects runtime environment and starts the application

set -e

echo "🏦 Starting Bank API..."

# Set environment variables
export ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
export PORT=${PORT:-5000}

# Check if dotnet is available
if ! command -v dotnet &> /dev/null; then
    echo "⚠️  .NET SDK not found in PATH"
    echo "📦 Attempting to use pre-published binaries..."
    
    # Check if application is already published
    if [ -f "src/Bank.Api/bin/Release/net9.0/Bank.Api.dll" ]; then
        echo "🚀 Starting from published binaries..."
        cd src/Bank.Api/bin/Release/net9.0
        exec dotnet Bank.Api.dll --urls="http://+:$PORT"
    else
        echo "❌ Error: .NET SDK not available and application not pre-published"
        echo "Please ensure this container has .NET 9.0 SDK installed"
        exit 1
    fi
else
    echo "📦 Publishing application..."
    cd src/Bank.Api
    dotnet publish -c Release -o ../../publish /p:UseAppHost=false
    
    echo "🚀 Starting application on port $PORT..."
    cd ../../publish
    exec dotnet Bank.Api.dll --urls="http://+:$PORT"
fi
