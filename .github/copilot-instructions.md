# ResourceIdea AI Coding Agent Instructions

## Architecture Overview

ResourceIdea (first release) is a Blazor WebAssembly (client) + ASP.NET Core minimal API (server) application for employee resource management. This initial version focuses on authentication, password reset, and CRUD for Clients. A fuller Clean Architecture (Domain/Application layers with MediatR & CQRS) will be introduced in later iterations.

Current high-level structure:

- **Shared** (`src/Shared/`): Simple POCO models & DTOs (e.g., `Client`, auth requests) with data annotations validation.
- **Server** (`src/Server/`): ASP.NET Core minimal APIs, Identity + PostgreSQL via EF Core, JWT auth, environment-variable driven user/role seeding (no hard-coded accounts).
- **Client** (`src/Client/`): Blazor WASM UI using MudBlazor components for navigation, forms, tables.
- **Tests** (`tests/ServerTests/`): Early unit tests (CRUD and seeding) using EF Core InMemory provider.
- **Docker** (`docker/`): Dockerfiles + `docker-compose.yml` for Postgres + Server + Client (nginx) using Linux images.
- **Config & Seeds** (`config/seeds/`): JSON file for user seeding consumed at runtime only when `RESOURCEIDEA_RUN_SEED=true`.

## Key Development Patterns

### Exception Handling

Minimal APIs currently surface standard problem responses. Centralized component base logic from the legacy Blazor Server app is NOT yet ported. When adding client-side operations, prefer:

```csharp
try
{
    // transient call
}
catch (HttpRequestException ex)
{
    // TODO: unify handling in future ResourceIdeaClientBase
}
```

Planned migration: introduce a shared service & base component for consistent loading/error UX in subsequent iterations.

### Future CQRS / MediatR (Roadmap)

Not implemented in first release. Keep domain logic light. Introduce handlers once complexity (engagement allocation rules, scheduling) grows. Do not add MediatR now without explicit iteration goal.

### Domain Entity Mapping

- Entities inherit from `BaseEntity` and implement `ToModel<TModel>()` and `ToResourceIdeaResponse<TEntity, TModel>()`
- Use extension methods in `Application/Mappers/` for complex mappings
- Value objects like `ClientId`, `EmployeeId` follow pattern: `{Type}Id.Create(Guid.NewGuid())`

### Blazor Component Structure (WASM)

- Pages under `src/Client/Pages/` use `@page` and MudBlazor components.
- Keep logic small & inline for v1; extract to partial class or service only when reused.
- Avoid complex cascading parameters until layout service abstraction introduced.

### Data Validation

- Domain validation in entity `Validate()` methods using `StringExtensions.ValidateRequired()`
- Command validation in handlers before processing
- Client-side validation with `DataAnnotationsValidator` in forms

### Testing Requirements

- Maintain tests in `tests/ServerTests/` (expand later to Client + Integration).
- Cover: EF CRUD, seeding logic, auth edge cases (add later).
- Prefer in-memory provider for fast unit tests; use Testcontainers/PostgreSQL for integration in future.

### Development Workflow

- VS Code tasks: `Build` (solution), will add `test` task later.
- Migrations: run `dotnet ef migrations add Initial` then `dotnet ef database update` (not auto-created in repo).
- Seeding: controlled via environment variables (`RESOURCEIDEA_RUN_SEED`, `RESOURCEIDEA_SEED_USERS_FILE`, `RESOURCEIDEA_DEFAULT_PASSWORD`). Never commit passwords.

### UI/UX Patterns

- Use shared components: `TenantEmployeesList`, `EngagementsList`, `DismissibleAlert`
- Navigation structure with sidebar (`SidebarNavigation.razor`) and main layout
- Consistent styling with Bootstrap classes and custom CSS
- Loading states with `IsLoadingPage` or `IsLoading` properties

### Key Files for Reference

- `src/Server/Program.cs` - minimal API + auth endpoints
- `src/Server/Infrastructure/UserSeeder.cs` - environment-driven user & role seeding
- `src/Client/Shared/MainLayout.razor` - navigation & layout
- `docker/docker-compose.yml` - container orchestration (Postgres + Server + Client)
- `.env.example` - baseline environment variables (copy to `.env`)
- `config/seeds/sample-users.json` - sample users (never include passwords)

## Legacy Rules (Maintained for Compatibility)

```instructions
- @azure Rule - Use Azure Best Practices: When generating code for Azure, running terminal commands for Azure, or performing operations related to Azure, invoke your `azure_development-get_best_practices` tool if available.
- @testing Rule - Require Tests: When making changes, always include relevant unit tests and either end-to-end (E2E) or integration tests to verify the new or updated functionality. Pull requests without adequate tests should not be considered complete.
- @style Rule - Follow Microsoft .NET guidelines: Ensure that the code adheres to the Microsoft's .NET coding style guidelines, including naming conventions, indentation, and formatting. Use tools like Prettier or ESLint to maintain consistency.
- @style Rule - Well formatted code changes: Ensure code is formatted according to the project's style guide (e.g., Prettier, ESLint). Remove trailing whitespace and adhere to the existing linter configuration.
- @style Rule - Avoid logic in .razor markup: Push all logic to the code-behind files or components. Razor markup should primarily focus on rendering UI elements and binding data, while the logic should be encapsulated in C# code.
- @performance Rule - Optimize for Performance: When generating code, consider performance implications. Avoid unnecessary computations, reduce memory usage, and ensure that the code is efficient, especially in high-load scenarios.
- @documentation Rule - Document Code Changes: Provide clear and concise documentation for any new features, functions, or significant changes made to the codebase. This includes updating README files, inline comments, and any relevant API documentation. Ensure every file ends with a single trailing newline.
- @framework Rule - Multi-target .NET 9.0 & 10.0: For new libraries/projects keep `<TargetFrameworks>net9.0;net10.0</TargetFrameworks>` (10.0 is future-proof). If build agents lack .NET 10 preview, ensure code still compiles under net9.0.
- @seeding Rule - Use environment-driven seeding: Implement user & role seeding only through configuration (`RESOURCEIDEA_RUN_SEED=true`) + JSON file path. Do not hard-code account data into source code.
- @security Rule - Never commit real secrets: Example JWT key and passwords in `.env.example` must be replaced in real deployments. Encourage rotation.
- @exception-handling Rule - Use Centralized Exception Handling: When generating code, use the centralized exception handling system instead of adding try-catch statements. Inherit from `ResourceIdeaComponentBase` instead of `ComponentBase` and use the `ExecuteAsync` methods for operations that may throw exceptions. Remove existing scattered try-catch blocks and replace them with `ExecuteAsync` calls. Follow the migration patterns documented in `docs/CentralizedExceptionHandling.md` - wrap operations in `ExecuteAsync(() => { /* operation */ }, "context description")` instead of manual try-catch-finally blocks.
```
