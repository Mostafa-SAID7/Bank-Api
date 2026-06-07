#!/bin/bash
set -e

echo "Starting Bank API..."

# Use PORT if set, otherwise default to 5000
PORT=${PORT:-5000}
export ASPNETCORE_URLS="http://+:${PORT}"

echo "Port: $PORT"
echo "Environment: ${ASPNETCORE_ENVIRONMENT:-Production}"

# Run the application
exec dotnet Bank.Api.dll
