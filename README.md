# Bank Management System

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=alert_status&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=bugs&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=code_smells&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=coverage&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=duplicated_lines_density&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=ncloc&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=sqale_rating&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=reliability_rating&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=security_rating&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=sqale_index&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Mostafa-SAID7_Bank-Api&metric=vulnerabilities&token=64cd308bb15671223d91856900d4d1e23843ad91)](https://sonarcloud.io/summary/new_code?id=Mostafa-SAID7_Bank-Api)

A comprehensive banking API built with .NET 9.0 and Clean Architecture principles.

## Features

- **Account Management**: Create and manage bank accounts
- **Transaction Processing**: Secure fund transfers and transaction history
- **User Authentication**: JWT-based authentication system
- **Administrative Tools**: Admin dashboard for system oversight
- **Security**: Industry-standard security measures and encryption

## Quick Start

### Prerequisites
- .NET 9.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Installation
```bash
git clone https://github.com/yourusername/bank-management-system.git
cd bank-management-system/bank-backend
dotnet restore
dotnet build
dotnet run --project Bank.Api
```

Visit `https://localhost:7000/swagger` to explore the API.

## Documentation

- [📋 Features](docs/FEATURES.md)
- [🏗️ Project Structure](docs/STRUCTURE.md)
- [⚙️ Setup Guide](docs/PROJECT_SETUP.md)
- [🚀 Deployment](docs/DEPLOYMENT.md)
- [🛠️ Technologies](docs/TECHNOLOGIES.md)
- [📖 Use Cases](docs/USE_CASES.md)
- [🤝 Contributing](docs/CONTRIBUTING.md)

## Architecture

This project follows Clean Architecture with:
- **API Layer**: Controllers and DTOs
- **Application Layer**: Business logic and services
- **Domain Layer**: Core entities and business rules
- **Infrastructure Layer**: Data access and external services

## Contributing

We welcome contributions! Please read our [Contributing Guide](docs/CONTRIBUTING.md) and [Code of Conduct](docs/CODE_OF_CONDUCT.md).

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

- 📧 Email: support@bankproject.com
- 🐛 Issues: [GitHub Issues](https://github.com/yourusername/bank-management-system/issues)
- 📖 Documentation: [Project Wiki](https://github.com/yourusername/bank-management-system/wiki)