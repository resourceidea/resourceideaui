# Developer Onboarding Guide

Welcome to **EastSeat.ResourceIdea**! This guide will help you get started from zero to running the application locally.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Initial Setup](#initial-setup)
3. [Running the Application](#running-the-application)
4. [Development Workflow](#development-workflow)
5. [Testing](#testing)
6. [Common Tasks](#common-tasks)
7. [Troubleshooting](#troubleshooting)
8. [Next Steps](#next-steps)

---

## Prerequisites

Before you begin, ensure you have the following installed on your machine:

### Required Software

1. **.NET 9 SDK**
   - Download: [https://dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
   - Verify installation: `dotnet --version` (should show 9.0.x)

2. **Git**
   - Download: [https://git-scm.com/downloads](https://git-scm.com/downloads)
   - Verify installation: `git --version`

3. **Code Editor**
   - Recommended: [Visual Studio Code](https://code.visualstudio.com/) with C# Dev Kit extension
   - Alternative: Visual Studio 2022 (17.8+), JetBrains Rider

4. **PostgreSQL 16+** (Choose one option)
   - **Option A (Recommended)**: Docker Desktop for containerized PostgreSQL
     - Download: [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)
     - Verify: `docker --version`
   - **Option B**: Native PostgreSQL installation
     - Download: [https://www.postgresql.org/download/](https://www.postgresql.org/download/)
     - Verify: `psql --version`

### Recommended Tools

- **pgAdmin 4** (included in Docker Compose, or download separately)
- **Postman** or **Thunder Client** for API testing (future)
- **PowerShell 7+** (Windows/Linux/macOS) for script execution

---

## Initial Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/resourceidea/resourceideaui.git
cd resourceideaui
```

### Step 2: Configure Environment Variables

Create a `.env` file in the project root by copying the sample:

```bash
# PowerShell
Copy-Item .env.sample .env

# Bash
cp .env.sample .env
```

Edit `.env` and update the values as needed:

```env
# Database Configuration
DB_HOST=localhost
DB_PORT=5432
DB_NAME=resourceidea_dev
DB_USER=postgres
DB_PASSWORD=YourSecurePassword123!

# Application Settings
ASPNETCORE_ENVIRONMENT=Development
SEED_ADMIN_USER=true
ADMIN_EMAIL=admin@eastseat.com
ADMIN_PASSWORD=Admin@123
RUN_MIGRATIONS_ON_STARTUP=true
```

**Important Security Note**: Change `ADMIN_PASSWORD` before deploying to production!

### Step 3: Choose Your Setup Method

You can set up the application using one of two methods:

#### **Method A: Automated Setup Script (Recommended for Beginners)**

This is the fastest way to get started. The script will:
- Check prerequisites
- Start PostgreSQL via Docker (optional)
- Restore NuGet packages
- Build the solution
- Create and apply database migrations
- Run tests
- Start the application

**On Windows (PowerShell):**
```powershell
.\setup.ps1
```

**On Linux/macOS (Bash):**
```bash
chmod +x setup.sh
./setup.sh
```

**Script Options:**
- Skip Docker if you have PostgreSQL installed: `.\setup.ps1 -SkipDocker`
- Skip migrations: `.\setup.ps1 -SkipMigrations`
- Production build: `.\setup.ps1 -Production`

After successful execution, the application will be running at **https://localhost:5001**

Skip to [Step 5: Access the Application](#step-5-access-the-application) if using the automated script.

---

#### **Method B: Manual Setup (Step-by-Step)**

Follow these steps if you want more control or if the automated script fails.

##### Step 3a: Start PostgreSQL Database

**If using Docker:**
```bash
docker-compose up -d
```

This starts:
- PostgreSQL 16 on port 5432
- pgAdmin 4 on port 5050 (http://localhost:5050)

Verify PostgreSQL is ready:
```bash
docker-compose ps
```

**If using native PostgreSQL:**
- Ensure PostgreSQL service is running
- Create database: `createdb resourceidea_dev -U postgres`

##### Step 3b: Restore Dependencies

```bash
dotnet restore EastSeat.ResourceIdea.slnx
```

##### Step 3c: Build the Solution

```bash
dotnet build EastSeat.ResourceIdea.slnx --configuration Debug
```

##### Step 3d: Create Initial Database Migration

Navigate to the Infrastructure project:

```bash
cd src/EastSeat.ResourceIdea.Infrastructure
```

Create the initial migration:

```bash
dotnet ef migrations add InitialCreate --startup-project ../EastSeat.ResourceIdea.Server --context ApplicationDbContext
```

##### Step 3e: Apply Database Migrations

```bash
dotnet ef database update --startup-project ../EastSeat.ResourceIdea.Server --context ApplicationDbContext
```

Return to project root:
```bash
cd ../..
```

##### Step 3f: Run Tests (Optional but Recommended)

```bash
dotnet test --no-build --verbosity normal
```

### Step 5: Access the Application

Open your browser and navigate to:
- **Application**: https://localhost:5001 or http://localhost:5000
- **pgAdmin** (if using Docker): http://localhost:5050
  - Email: `admin@admin.com`
  - Password: `admin`

### Step 6: Login with Admin Account

Use the credentials from your `.env` file:
- **Email**: admin@eastseat.com (or your custom value)
- **Password**: Admin@123 (or your custom value)

---

## Running the Application

### Development Mode with Hot Reload

For active development with automatic recompilation on file changes:

**PowerShell:**
```powershell
.\scripts\dev-watch.ps1
```

**Bash:**
```bash
./scripts/dev-watch.sh
```

This runs `dotnet watch run` in the Server project, enabling hot reload for C# and Razor files.

### Standard Run

```bash
cd src/EastSeat.ResourceIdea.Server
dotnet run
```

### Stop the Application

Press `Ctrl+C` in the terminal where the application is running.

### Stop Docker Containers

```bash
docker-compose down
```

To remove volumes (database data):
```bash
docker-compose down -v
```

---

## Development Workflow

### Project Structure

```
resourceideaui/
├── src/
│   ├── EastSeat.ResourceIdea.Domain/       # Entities, Value Objects, Enums
│   ├── EastSeat.ResourceIdea.Application/  # Services, DTOs, Interfaces
│   ├── EastSeat.ResourceIdea.Infrastructure/ # DbContext, Identity, EF Config
│   └── EastSeat.ResourceIdea.Server/       # Blazor Server UI (MudBlazor)
├── tests/
│   ├── EastSeat.ResourceIdea.Domain.Tests/
│   ├── EastSeat.ResourceIdea.Application.Tests/
│   └── EastSeat.ResourceIdea.Integration.Tests/
├── docs/                                   # Documentation
├── scripts/                                # Utility scripts
├── .github/                                # CI/CD workflows
├── docker-compose.yml                      # PostgreSQL + pgAdmin
├── setup.ps1 / setup.sh                    # Automated setup scripts
└── EastSeat.ResourceIdea.slnx              # Primary solution file (synced to .sln for tooling)
```

### Architecture Overview

This project follows **Clean Architecture** principles:

1. **Domain Layer**: Pure business logic, entities with factory methods, domain services
   - No dependencies on other layers
   - Example: `Client.Create()`, `CalendarService.CalculateWorkingDays()`

2. **Application Layer**: Use case orchestration via application services
   - **No MediatR** - we use simple service classes with method-per-use-case
   - Example: `ClientService.CreateClientAsync()`, `PlannerService.GetPlannerYearAsync()`

3. **Infrastructure Layer**: Data access, EF Core, ASP.NET Identity
   - `ApplicationDbContext` with 9 DbSets
   - Custom `ApplicationUser` and `ApplicationRole`
   - Entity configurations for relationships and indexes

4. **Server Layer**: Blazor Server UI with MudBlazor components
   - Interactive server rendering
   - Cookie-based authentication
   - Policy-based authorization

### Coding Conventions

Please read [`.github/copilot-instructions.md`](../.github/copilot-instructions.md) for comprehensive guidelines. Key points:

- **Nullable Reference Types Enabled**: Treat warnings seriously
- **Explicit Access Modifiers**: Always specify `public`, `private`, `internal`
- **Immutable Domain Models**: Favor records and value objects
- **Entity Factory Methods**: Use static `Create()` methods with `Result<T>` pattern
- **Guard Clauses**: Validate inputs early using `Guard` class
- **Small Methods**: Keep methods under ~40 lines; extract helpers
- **Functional LINQ**: Readable chains; break into local variables if complex
- **Async for I/O Only**: Don't overuse async across entire stack

### Key Domain Rules

1. **Assignments**: Employee allocation cannot exceed 100% on any working day
2. **Leave Management**: Leave overrides assignment availability
3. **Working Days**: Weekends (Sat/Sun) and Public Holidays are non-working
4. **Rollover**: Duplicates EngagementYear + Assignments with leap-year adjustments (Feb 29 → Feb 28)

---

## Testing

### Run All Tests

**PowerShell:**
```powershell
.\scripts\test.ps1
```

**Bash:**
```bash
./scripts/test.sh
```

### Run Tests with Coverage

**PowerShell:**
```powershell
.\scripts\test.ps1 -Coverage
```

**Bash:**
```bash
./scripts/test.sh --coverage
```

Coverage reports are generated in `./coverage/` directory.

### Run Tests in Watch Mode

**PowerShell:**
```powershell
.\scripts\test.ps1 -Watch
```

**Bash:**
```bash
./scripts/test.sh --watch
```

### Writing Tests

- Use **xUnit** for all tests
- Follow **Arrange-Act-Assert** pattern
- Keep tests deterministic (avoid `DateTime.Now`, use `ITimeProvider` abstraction)
- Test edge cases (leap years, overlapping assignments, leave on weekends)

Example domain test:

```csharp
[Fact]
public void Create_WithValidData_ShouldSucceed()
{
    // Arrange
    var name = "Acme Ltd";
    var registrationNumber = "REG123";

    // Act
    var result = Client.Create(name, registrationNumber, null, null);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(name, result.Value.Name);
}
```

---

## Common Tasks

### Adding a New Database Migration

**PowerShell:**
```powershell
.\scripts\migration.ps1 add -Name YourMigrationName
```

**Bash:**
```bash
./scripts/migration.sh add YourMigrationName
```

### Applying Migrations

**PowerShell:**
```powershell
.\scripts\migration.ps1 update
```

**Bash:**
```bash
./scripts/migration.sh update
```

### Listing Migrations

**PowerShell:**
```powershell
.\scripts\migration.ps1 list
```

**Bash:**
```bash
./scripts/migration.sh list
```

### Removing Last Migration

**PowerShell:**
```powershell
.\scripts\migration.ps1 remove
```

**Bash:**
```bash
./scripts/migration.sh remove
```

### Clean Build Artifacts

**PowerShell:**
```powershell
.\scripts\clean.ps1
```

**Bash:**
```bash
./scripts/clean.sh
```

To also remove Docker volumes:

**PowerShell:**
```powershell
.\scripts\clean.ps1 -All
```

**Bash:**
```bash
./scripts/clean.sh --all
```

### Adding a New NuGet Package

```bash
cd src/EastSeat.ResourceIdea.[ProjectName]
dotnet add package PackageName
```

**Important**: Avoid paid or proprietary packages. Justify any new dependency in your PR.

### Creating a New Entity

1. Create entity in `src/EastSeat.ResourceIdea.Domain/Entities/`
2. Add factory method: `public static Result<YourEntity> Create(...)`
3. Add validation using `Guard` class
4. Create entity configuration in `src/EastSeat.ResourceIdea.Infrastructure/Data/Configurations/`
5. Add `DbSet<YourEntity>` to `ApplicationDbContext`
6. Create migration: `.\scripts\migration.ps1 add AddYourEntity`
7. Write unit tests in `tests/EastSeat.ResourceIdea.Domain.Tests/`

### Creating a New Application Service

1. Create service in `src/EastSeat.ResourceIdea.Application/Services/`
2. Follow pattern: one public method per use case
3. Inject `IApplicationDbContext` and `ILogger<T>`
4. Return `Result<T>` or DTO
5. Register service in `Program.cs`: `builder.Services.AddScoped<IYourService, YourService>();`
6. Write tests in `tests/EastSeat.ResourceIdea.Application.Tests/`

---

## Troubleshooting

### "dotnet: command not found"

**Solution**: Install .NET 9 SDK from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

### "docker: command not found"

**Solution**: 
- Install Docker Desktop: [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)
- Or use `-SkipDocker` flag and install PostgreSQL natively

### "Could not connect to the database"

**Solution**:
1. Verify PostgreSQL is running: `docker-compose ps` (should show "Up")
2. Check `.env` file has correct `DB_HOST`, `DB_PORT`, `DB_USER`, `DB_PASSWORD`
3. Wait 30 seconds for PostgreSQL to fully initialize
4. Test connection: `docker-compose exec postgres pg_isready -U postgres`

### "No migrations configuration type was found"

**Solution**: Run migration commands from Infrastructure project directory:
```bash
cd src/EastSeat.ResourceIdea.Infrastructure
dotnet ef migrations add YourMigration --startup-project ../EastSeat.ResourceIdea.Server
cd ../..
```

Or use the helper script: `.\scripts\migration.ps1 add -Name YourMigration`

### "Port 5432 already in use"

**Solution**:
1. Stop existing PostgreSQL service: `docker stop $(docker ps -q --filter "expose=5432")`
2. Or change `DB_PORT` in `.env` and `docker-compose.yml` to a different port (e.g., 5433)

### Build Errors After Pull

**Solution**:
```bash
.\scripts\clean.ps1
dotnet restore
dotnet build
```

### "Cannot access MudBlazor components"

**Solution**: Ensure `_Imports.razor` contains:
```razor
@using MudBlazor
```

And `App.razor` references MudBlazor CSS/JS:
```html
<link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" rel="stylesheet" />
<link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />
<script src="_content/MudBlazor/MudBlazor.min.js"></script>
```

### Tests Failing Due to Database State

**Solution**:
- Domain tests should NOT depend on database
- Application tests should use `InMemory` provider or test-specific database
- Integration tests should reset database between test runs
- Consider using Testcontainers for isolated PostgreSQL instances

---

## Next Steps

Congratulations! You now have the application running. Here's what to explore next:

### 1. **Explore the Codebase**

- Read [`README.md`](../README.md) for architecture overview
- Review [`.github/copilot-instructions.md`](../.github/copilot-instructions.md) for contribution guidelines
- Examine `src/EastSeat.ResourceIdea.Domain/Entities/` to understand core business models
- Study `src/EastSeat.ResourceIdea.Server/Program.cs` to see DI configuration

### 2. **Work on Your First Feature**

- Check GitHub Issues for "good first issue" labels
- Create a feature branch: `git checkout -b feature/your-feature-name`
- Follow the coding conventions in copilot-instructions.md
- Write tests for your changes
- Submit a Pull Request

### 3. **Understand Key Features**

- **Client & Engagement Management**: Browse to `/clients` and `/engagements` (when implemented)
- **Resource Planner**: Navigate to `/planner` to see year-view allocations (under development)
- **Leave Management**: Visit `/leave` to request time off
- **Public Holidays**: Manage holidays at `/holidays`

### 4. **Database Exploration**

Access pgAdmin at http://localhost:5050:
- Add server: Host `postgres`, Port `5432`, Database `resourceidea_dev`, User `postgres`
- Explore tables: `Clients`, `Engagements`, `EngagementYears`, `Employees`, `Assignments`
- View Identity tables: `Users`, `Roles`, `UserRoles`

### 5. **Learn About Azure Deployment**

- Review deployment section in [`README.md`](../README.md)
- Study CI/CD workflows in `.github/workflows/`
- Understand environment-specific settings in `appsettings.Production.json`

### 6. **Join the Community**

- Read [`CONTRIBUTING.md`](../CONTRIBUTING.md) (when created)
- Review open Pull Requests to see coding patterns
- Participate in code reviews
- Ask questions in GitHub Discussions

---

## Additional Resources

### Documentation

- [ASP.NET Core Blazor](https://learn.microsoft.com/aspnet/core/blazor/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [MudBlazor Components](https://mudblazor.com/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [xUnit Testing](https://xunit.net/)

### Internal Docs

- [`README.md`](../README.md) - Project overview and setup
- [`.github/copilot-instructions.md`](../.github/copilot-instructions.md) - Architecture and coding guidelines
- [`LICENSE`](../LICENSE) - MIT License

### Getting Help

- **GitHub Issues**: Report bugs or request features
- **Pull Requests**: Submit code reviews and questions
- **Email**: admin@eastseat.com (for security issues)

---

## Checklist: Am I Ready to Develop?

Before starting development, ensure:

- [ ] .NET 9 SDK installed (`dotnet --version` shows 9.0.x)
- [ ] Git installed and repository cloned
- [ ] Code editor set up (VS Code with C# Dev Kit recommended)
- [ ] PostgreSQL running (Docker or native)
- [ ] `.env` file created and configured
- [ ] Database migrations applied
- [ ] Application starts successfully at https://localhost:5001
- [ ] Can log in with admin credentials
- [ ] Tests run successfully (`.\scripts\test.ps1`)
- [ ] Read `.github/copilot-instructions.md` architecture guidelines
- [ ] Understand "No MediatR" application service pattern
- [ ] Familiar with `Result<T>` pattern and entity factory methods

If all checked, you're ready to contribute! 🚀

---

## FAQ

**Q: Why no MediatR or CQRS frameworks?**  
A: We prefer simplicity and maintainability. Application services with method-per-use-case provide clear boundaries without additional abstraction layers.

**Q: Can I use Visual Studio instead of VS Code?**  
A: Yes! Visual Studio 2022 (17.8+) fully supports .NET 9 and Blazor Server.

**Q: How do I reset the database?**  
A: Run `docker-compose down -v` (removes volumes), then `.\setup.ps1` to recreate.

**Q: Where are the authentication pages?**  
A: Currently under development. Basic Identity is configured; UI pages will be added in future sprints.

**Q: Can I contribute without Azure knowledge?**  
A: Absolutely! Most development is local. Azure deployment is optional for contributors.

**Q: How do I handle merge conflicts?**  
A: Pull latest `main`, resolve conflicts in your feature branch, run tests, then push.

**Q: What if setup.ps1 fails on Linux?**  
A: Use `setup.sh` instead. Ensure it's executable: `chmod +x setup.sh`

---

**Welcome to the team! Happy coding!** 🎉

If you encounter any issues not covered here, please open a GitHub Issue or contact the maintainers.

---

*Last Updated: November 2025*  
*Version: 1.0*
