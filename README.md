# EastSeat.ResourceIdea

A comprehensive resource planning SaaS application for audit and tax advisory firms in Uganda, built with ASP.NET Blazor Server targeting **.NET 10**.

## Overview

ResourceIdea streamlines resource allocation, engagement management, and leave tracking for audit firms. The application provides:

- **Client & Engagement Management**: Track clients and their audit/tax engagements
- **Resource Planning**: Visual year-long planner showing employee assignments across engagements
- **Leave Management**: Comprehensive time-off tracking with approval workflows
- **Year Rollover**: Automatically copy engagement assignments to the next year
- **Public Holiday Management**: Configure region-specific public holidays
- **Authorization & Identity**: Role-based access with Partner, Manager, and Staff roles

## Architecture

The solution follows clean architecture principles with clear separation of concerns:

```
EastSeat.ResourceIdea/
├── src/
│   ├── EastSeat.ResourceIdea.Domain/          # Core domain entities and business logic
│   ├── EastSeat.ResourceIdea.Application/     # Application services and DTOs
│   ├── EastSeat.ResourceIdea.Infrastructure/  # EF Core, Identity, data access
│   └── EastSeat.ResourceIdea.Server/          # Blazor Server UI
├── tests/
    ├── EastSeat.ResourceIdea.Domain.Tests/
    ├── EastSeat.ResourceIdea.Application.Tests/
    └── EastSeat.ResourceIdea.Integration.Tests/
├── docs/
│   └── ONBOARDING.md                          # Developer onboarding guide
├── scripts/
│   ├── dev-watch.ps1/.sh                      # Hot reload development
│   ├── test.ps1/.sh                           # Test runner with coverage
│   ├── clean.ps1/.sh                          # Clean build artifacts
│   └── migration.ps1/.sh                      # Database migration management
├── setup.ps1/.sh                              # Automated setup scripts
└── docker-compose.yml                         # PostgreSQL + pgAdmin
```

### Key Design Principles

- **No MediatR**: Uses simple application service pattern (method-per-use-case)
- **Functional C# style**: LINQ where appropriate, readable code over complexity
- **Domain-driven entities**: Factory methods, guard clauses, invariant enforcement
- **PostgreSQL**: Production-ready relational database
- **Custom Identity**: No scaffolded UI; custom user and role classes
- **MudBlazor**: Material Design UI components

## Technology Stack

- **.NET 10** – Target framework (`net10.0`)
- **ASP.NET Blazor Server** – Interactive server-side rendering
- **Entity Framework Core 9** – ORM with PostgreSQL provider (packages v9.x)
- **ASP.NET Identity** – Authentication and authorization
- **MudBlazor** – UI component library
- **xUnit** – Testing framework
- **PostgreSQL** – Database (via Npgsql)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (earlier versions will fail to build `net10.0` projects)
- [PostgreSQL 16+](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/get-started) (optional, for local database)

## Getting Started

### 1. Clone the Repository
### Quick Start (Automated Setup)

For detailed step-by-step instructions, see the **[Developer Onboarding Guide](docs/ONBOARDING.md)**.

**Prerequisites**: .NET 10 SDK, Git, Docker (or PostgreSQL 16+)

1. **Clone the repository**

```bash
git clone https://github.com/resourceidea/resourceideaui.git
cd resourceideaui
```

2. **Configure environment**

```bash
# PowerShell
Copy-Item .env.sample .env

# Bash
cp .env.sample .env
```

Edit `.env` with your settings (database credentials, admin password, etc.)

3. **Run automated setup**

**Windows (PowerShell):**
```powershell
.\setup.ps1
```

**Linux/macOS (Bash):**
```bash
chmod +x setup.sh
./setup.sh
```

The script will:
- ✅ Check prerequisites (.NET SDK, Docker)
- ✅ Start PostgreSQL via Docker
- ✅ Restore NuGet packages and build
- ✅ Create and apply database migrations
- ✅ Run tests
- ✅ Start the application at **https://localhost:5001**

**Script Options:**
- Skip Docker: `./setup.ps1 -SkipDocker` (if you have PostgreSQL installed)
- Skip migrations: `./setup.ps1 -SkipMigrations`
- Production build: `./setup.ps1 -Production`

4. **Login**

Default admin credentials:
- Email: `admin@eastseat.com`
- Password: `Admin@123` (or your custom `ADMIN_PASSWORD` from `.env`)

### Manual Setup

If you prefer manual setup or the automated script fails, see the [Manual Setup section](docs/ONBOARDING.md#method-b-manual-setup-step-by-step) in the onboarding guide.

## Development

### Build the Solution

The repository uses an XML manifest (`EastSeat.ResourceIdea.slnx`) instead of a native `.sln` for maintainability. A generator script materializes the real `.sln` before builds.

Generate + build (PowerShell):
```powershell
pwsh ./tools/Generate-Solution.ps1
dotnet build ./EastSeat.ResourceIdea.sln
```

Generate + build (Bash):
```bash
pwsh ./tools/Generate-Solution.ps1
dotnet build ./EastSeat.ResourceIdea.sln
```

### Run Tests

```powershell
dotnet test ./EastSeat.ResourceIdea.sln
```

### Add a Migration

```powershell
cd src/EastSeat.ResourceIdea.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../EastSeat.ResourceIdea.Server
```

### Watch Mode (Hot Reload)

```powershell
cd src/EastSeat.ResourceIdea.Server
dotnet watch run
```
### Development Scripts

The project includes utility scripts for common development tasks:

#### Hot Reload (Watch Mode)

Automatic recompilation on file changes:

**PowerShell:**

```powershell
.\scripts\dev-watch.ps1
```

**Bash:**

```bash
./scripts/dev-watch.sh
```

#### Run Tests

**PowerShell:**

```powershell
.\scripts\test.ps1           # Run all tests
.\scripts\test.ps1 -Coverage  # With coverage report
.\scripts\test.ps1 -Watch     # Watch mode
```

**Bash:**

```bash
./scripts/test.sh            # Run all tests
./scripts/test.sh --coverage # With coverage report
./scripts/test.sh --watch    # Watch mode
```

#### Database Migrations

**PowerShell:**

```powershell
.\scripts\migration.ps1 add -Name MigrationName  # Create migration
.\scripts\migration.ps1 update                   # Apply migrations
.\scripts\migration.ps1 list                     # List migrations
.\scripts\migration.ps1 remove                   # Remove last migration
```

**Bash:**

```bash
./scripts/migration.sh add MigrationName  # Create migration
./scripts/migration.sh update             # Apply migrations
./scripts/migration.sh list               # List migrations
./scripts/migration.sh remove             # Remove last migration
```

#### Clean Build Artifacts

**PowerShell:**

```powershell
.\scripts\clean.ps1      # Clean bin/obj/coverage/TestResults
.\scripts\clean.ps1 -All # Also remove Docker volumes
```

**Bash:**

```bash
./scripts/clean.sh       # Clean bin/obj/coverage/TestResults
./scripts/clean.sh --all # Also remove Docker volumes
```

### Manual Commands

#### Generate & Build the Solution (Manual)

```bash
pwsh ./tools/Generate-Solution.ps1
dotnet build EastSeat.ResourceIdea.sln
```

#### Run Tests

```bash
dotnet test EastSeat.ResourceIdea.sln
```

#### Standard Run

```bash
cd src/EastSeat.ResourceIdea.Server
dotnet run
```

## Project Structure

### Domain Layer (`EastSeat.ResourceIdea.Domain`)

Contains pure domain logic:

- **Entities**: `Client`, `Engagement`, `EngagementYear`, `Employee`, `Assignment`, `LeaveRequest`, `PublicHoliday`, `RolloverHistory`
- **Enums**: `EngagementType`, `EngagementStatus`, `LeaveType`, etc.
- **Services**: `CalendarService` for working-day calculations
- **Common**: `AuditableEntity`, `Result<T>`, `Guard` clauses

### Application Layer (`EastSeat.ResourceIdea.Application`)

Application services orchestrating use cases:

- **Services**: `ClientService`, `EngagementService`, `PlannerService`, `RolloverService`
- **DTOs**: Request/response objects for API boundaries
- **Interfaces**: `IApplicationDbContext`, `ICurrentUserService`

### Infrastructure Layer (`EastSeat.ResourceIdea.Infrastructure`)

Data access and external concerns:

- **Data**: `ApplicationDbContext`, EF Core configurations
- **Identity**: `ApplicationUser`, `ApplicationRole`, policy definitions
- **Seeding**: Role and admin user seeding

### Server Layer (`EastSeat.ResourceIdea.Server`)

Blazor Server UI:

- **Components**: Razor components (pages, layouts)
- **Program.cs**: DI registration and middleware pipeline
- **appsettings**: Configuration files

## Deployment

### Azure App Service (Linux)

1. **Create Azure Resources**

```powershell
az group create --name rg-resourceidea --location eastus
az postgres flexible-server create --resource-group rg-resourceidea --name resourceidea-db --location eastus
az webapp create --resource-group rg-resourceidea --plan asp-resourceidea --name resourceidea-app --runtime "DOTNETCORE:10.0"
```

2. **Configure Connection String**

Set `DefaultConnection` in App Service configuration or use Azure Key Vault.

3. **Deploy**

```powershell
dotnet publish -c Release
az webapp deploy --resource-group rg-resourceidea --name resourceidea-app --src-path ./publish.zip
```

4. **Run Migrations**

Set `RUN_MIGRATIONS_ON_STARTUP=true` in App Service settings or run via Azure CLI.

## Testing Strategy

- **Domain Tests**: Pure logic (entity factories, calendar calculations, rollover date mapping)
- **Application Tests**: Service orchestration with in-memory or real database
- **Integration Tests**: End-to-end scenarios with Testcontainers (future)
- **UI Tests**: bUnit component tests (future)

Run tests with coverage:

```powershell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Test Coverage

Run tests with coverage using the helper script:

```bash
# PowerShell
.\scripts\test.ps1 -Coverage

# Bash
./scripts/test.sh --coverage
```

Coverage reports are generated in the `./coverage/` directory.

Or use dotnet CLI directly:

```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory:"./coverage"
```

## Key Features

### Resource Planner

Year-long timeline view showing:

- Employee assignments to engagements (color-coded)
- Leave periods (sick, PTO, maternity, etc.)
- Weekends (gray background)
- Public holidays (special color)
- Allocation percentages per day

Navigation by year with horizontal scrolling through months.

### Year Rollover

Administrators can rollover engagements and assignments:

- Dry-run preview showing what will be copied
- Excludes terminated employees
- Handles leap-year adjustments (Feb 29 → Feb 28)
- Maintains allocation percentages
- Creates audit log (`RolloverHistory`)

### Leave Management

Employees request leave; managers approve:

- Multiple leave types (PTO, sick, maternity, study, etc.)
- Automatic working-day calculation
- Integrates with resource planner (blocks assignment allocation)
- Email notifications (future)

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Follow the [Contribution Guidelines](.github/copilot-instructions.md).

### For New Contributors

1. Read the **[Developer Onboarding Guide](docs/ONBOARDING.md)** for complete setup instructions
2. Review the [Architecture Guidelines](.github/copilot-instructions.md) for coding conventions
3. Look for "good first issue" labels in GitHub Issues

### Contribution Workflow

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes following the coding conventions
4. Write tests for your changes
5. Run tests: `./scripts/test.ps1` (PowerShell) or `./scripts/test.sh` (Bash)
6. Commit your changes (`git commit -m 'Add amazing feature'`)
7. Push to the branch (`git push origin feature/amazing-feature`)
8. Open a Pull Request with:
    - Clear description of changes
    - Link to related issue
    - Test coverage notes
    - Manual verification steps

### Key Guidelines

- **No MediatR**: Use application service pattern (method-per-use-case)
- **Nullable enabled**: Treat warnings seriously
- **Entity factory methods**: Use `Result<T>` pattern with `Create()` methods
- **Guard clauses**: Validate inputs early
- **Small methods**: Keep under ~40 lines
- **Tests required**: xUnit with Arrange-Act-Assert pattern

See [Contribution Guidelines](.github/copilot-instructions.md) for complete details.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support & Help

## Documentation

- **[Developer Onboarding Guide](docs/ONBOARDING.md)** – Complete setup guide from zero to running application
- **[Startup & Runtime Guide](docs/AppStartup.md)** – Build & run details and runtime composition
- **[Architecture Guidelines](.github/copilot-instructions.md)** – Coding conventions and contribution rules
- **[README](README.md)** – This file (project overview)

## Support

For issues and questions:

- **New Developers**: Start with the [Onboarding Guide](docs/ONBOARDING.md)
- GitHub Issues: [https://github.com/resourceidea/resourceideaui/issues](https://github.com/resourceidea/resourceideaui/issues)
- Email: [admin@eastseat.com](mailto:admin@eastseat.com)

## Roadmap

- [ ] Multi-tenancy support
- [ ] Real-time planner updates (SignalR)
- [ ] Mobile app (MAUI)
- [ ] Advanced reporting and analytics
- [ ] Integration with accounting systems
- [ ] AI-powered capacity forecasting

---

Built with ❤️ by the EastSeat team for the audit and tax advisory community in Uganda.
