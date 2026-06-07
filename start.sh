#!/bin/bash

# Bank API startup script for Railpack deployment
# This script handles building and running the .NET application

set -e

echo "🏦 Starting Bank API deployment..."

# Change to the API project directory
cd src/Bank.Api

# Set environment variables
export ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}

echo "📦 Building application..."
dotnet build --configuration Release

echo "🚀 Starting application..."
dotnet run --configuration Release --no-build

# Exit with the same code as the dotnet application
exit $?
