#!/bin/bash

# Bank API startup script
# Runs the pre-published .NET application (built by Docker)

set -e

echo "🏦 Starting Bank API..."

# Set environment variables
export ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
export PORT=${PORT:-5000}
export ASPNETCORE_URLS="http://+:${PORT}"

# Ensure the app binary exists
if [ ! -f "/app/Bank.Api.dll" ]; then
    echo "❌ Error: Bank.Api.dll not found at /app/"
    echo "Docker build likely failed. Check build logs."
    exit 1
fi

echo "✅ Found application binary"
echo "🚀 Starting on port $PORT..."

# Run the application
exec dotnet /app/Bank.Api.dll
