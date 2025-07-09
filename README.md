# ResourceIdea

**ResourceIdea** is a comprehensive employee and resource management application built with Blazor Server and Clean Architecture. The application provides organizations with tools to efficiently manage their workforce, client relationships, and resource planning capabilities.

## 🚀 Features

### Core Management Capabilities
- **Employee Management**: Comprehensive employee profiles, department organization, and role assignments
- **Client Management**: Client relationship tracking, engagement management, and work item organization
- **Resource Planning**: Timeline views, resource allocation, and service line management
- **Department Management**: Organizational structure with department hierarchy and management
- **Job Position Management**: Role definitions, responsibilities, and position hierarchies
- **Engagement Management**: Project engagements, timeline tracking, and deliverable management
- **Work Item Management**: Task organization, status tracking, and priority management

### Technical Features
- **Multi-tenant Architecture**: Secure tenant isolation with subscription-based access
- **Centralized Exception Handling**: Consistent error management across all components
- **Identity Management**: Secure authentication and authorization with role-based access
- **Real-time Updates**: Blazor Server components with interactive server-side rendering
- **Responsive Design**: Modern UI with Bootstrap components and responsive layout

## 🏗️ Architecture

The application follows Clean Architecture principles with clear separation of concerns:

### Layer Structure
```
├── Domain/              # Business entities, value objects, and domain logic
├── Application/         # Use cases, commands, queries, and business rules
├── Infrastructure/      # Data access, external services, and cross-cutting concerns
│   ├── DataStore/       # Entity Framework Core data access layer
│   └── Migration/       # Database migration utilities
└── Web/                 # Blazor Server UI components and presentation logic
```

### Key Architectural Patterns
- **CQRS with MediatR**: Command Query Responsibility Segregation for clean business logic
- **Repository Pattern**: Abstracted data access with Entity Framework Core
- **Value Objects**: Domain-driven design with strongly-typed identifiers
- **Centralized Exception Handling**: Consistent error management and user experience
- **Dependency Injection**: Loose coupling with built-in .NET dependency injection

## 🛠️ Technology Stack

### Backend
- **.NET 9.0**: Latest .NET framework with enhanced performance
- **ASP.NET Core**: Web framework with Blazor Server components
- **Entity Framework Core**: Object-relational mapping for data access
- **SQL Server**: Primary database for data storage
- **MediatR**: Mediator pattern implementation for CQRS
- **ASP.NET Core Identity**: Authentication and authorization

### Frontend
- **Blazor Server**: Server-side rendering with real-time UI updates
- **Bootstrap**: Responsive CSS framework for modern UI
- **Font Awesome**: Icon library for enhanced user experience
- **JavaScript Interop**: Client-side interactions when needed

### Development & Testing
- **xUnit**: Unit testing framework
- **Moq**: Mocking framework for unit tests
- **In-Memory Database**: Testing with Entity Framework Core in-memory provider

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) or [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/resourceidea/resourceideaui.git
cd resourceideaui
```

### 2. Install .NET 9.0 SDK
The repository includes a .NET installation script:
```bash
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.102
export PATH="$HOME/.dotnet:$PATH"
```

### 3. Database Setup
1. Create a SQL Server database for the application
2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ResourceIdea;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 4. Build and Run
```bash
# Build the solution
dotnet build src/dev/Web/EastSeat.ResourceIdea.Web/EastSeat.ResourceIdea.Web.csproj

# Run the application
dotnet run --project src/dev/Web/EastSeat.ResourceIdea.Web/EastSeat.ResourceIdea.Web.csproj
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## 🧪 Testing

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Projects
```bash
# Domain tests
dotnet test src/tests/EastSeat.ResourceIdea.Domain.UnitTests/

# Application tests
dotnet test src/tests/EastSeat.ResourceIdea.Application.UnitTests/

# Web tests
dotnet test src/tests/EastSeat.ResourceIdea.Web.UnitTests/

# DataStore tests
dotnet test src/tests/EastSeat.ResourceIdea.DataStore.UnitTests/
```

## 📁 Project Structure

```
src/
├── dev/
│   ├── Core/
│   │   ├── EastSeat.ResourceIdea.Domain/       # Domain entities and business logic
│   │   └── EastSeat.ResourceIdea.Application/  # Application services and handlers
│   ├── Infrastructure/
│   │   └── EastSeat.ResourceIdea.DataStore/    # Data access layer
│   └── Web/
│       └── EastSeat.ResourceIdea.Web/          # Blazor Server application
└── tests/
    ├── EastSeat.ResourceIdea.Domain.UnitTests/
    ├── EastSeat.ResourceIdea.Application.UnitTests/
    ├── EastSeat.ResourceIdea.DataStore.UnitTests/
    └── EastSeat.ResourceIdea.Web.UnitTests/
```

## 💡 Development Guidelines

### Code Organization
- Follow Clean Architecture principles
- Use CQRS pattern for business operations
- Implement proper error handling with centralized exception management
- Write comprehensive unit tests for all business logic
- Use strongly-typed value objects for domain identifiers

### Component Development
- Inherit from `ResourceIdeaComponentBase` for Blazor components
- Use `ExecuteAsync` methods for operations that may throw exceptions
- Follow existing patterns for command and query handlers
- Implement proper validation in domain entities and commands

### Testing Standards
- Write unit tests for all handlers and business logic
- Use meaningful test names that describe the scenario
- Mock external dependencies using Moq
- Test both success and error scenarios

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-feature`)
3. Make your changes following the development guidelines
4. Add or update tests as needed
5. Ensure all tests pass (`dotnet test`)
6. Commit your changes (`git commit -am 'Add new feature'`)
7. Push to the branch (`git push origin feature/new-feature`)
8. Create a Pull Request

## 📖 Documentation

Additional documentation is available in the `/docs` directory:
- [Centralized Exception Handling](docs/CentralizedExceptionHandling.md)

## 🔒 Security

- Authentication and authorization using ASP.NET Core Identity
- Role-based access control
- Secure multi-tenant data isolation
- Input validation and sanitization
- HTTPS enforcement in production

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Team

ResourceIdea is developed and maintained by the EastSeat development team.

---

**ResourceIdea** - Streamlining organizational resource management with modern web technologies.
