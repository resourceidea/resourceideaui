# ResourceIdea AI Coding Agent Instructions

## Architecture Overview

ResourceIdea is a Blazor Server application for employee resource management with Clean Architecture. The solution consists of:

- **Domain Layer** (`Domain/`): Entities, value objects, models with business logic validation
- **Application Layer** (`Application/`): CQRS with MediatR, command/query handlers, mappers
- **Infrastructure Layer** (`DataStore/`, `Migration/`): EF Core data access, migration services
- **Web Layer** (`Web/`): Blazor Server components with centralized exception handling

## Key Development Patterns

### Exception Handling - Use Centralized System

- **ALWAYS** inherit from `ResourceIdeaComponentBase` instead of `ComponentBase` for Blazor components
- Use `ExecuteAsync(() => { /* operation */ }, "context description")` instead of try-catch blocks
- Leverage automatic loading state and error message management
- See `docs/CentralizedExceptionHandling.md` for migration patterns

```csharp
// Correct pattern
public partial class MyComponent : ResourceIdeaComponentBase
{
    private async Task LoadData()
    {
        await ExecuteAsync(async () =>
        {
            var result = await Mediator.Send(query);
            // Process result
        }, "Loading data");
    }
}
```

### CQRS Commands & Queries

- All operations use MediatR with handlers in `Application/Features/{Entity}/Handlers/`
- Commands inherit from `BaseRequest<TModel>` and implement `Validate()` method
- Handlers inherit from `BaseHandler` and return `ResourceIdeaResponse<T>`
- Use existing command/query patterns: `Get{Entity}ByIdQuery`, `Update{Entity}Command`, etc.

### Domain Entity Mapping

- Entities inherit from `BaseEntity` and implement `ToModel<TModel>()` and `ToResourceIdeaResponse<TEntity, TModel>()`
- Use extension methods in `Application/Mappers/` for complex mappings
- Value objects like `ClientId`, `EmployeeId` follow pattern: `{Type}Id.Create(Guid.NewGuid())`

### Blazor Component Structure

- Components use `@page` directive with proper routing (`/entity/{id:guid}`)
- Inject `IMediator`, `IResourceIdeaRequestContext`, `NavigationManager` as needed
- Use parameter validation: `[Parameter] public Guid Id { get; set; }`
- Follow navigation patterns: back buttons with `GetBackNavigationUrl()` methods

### Data Validation

- Domain validation in entity `Validate()` methods using `StringExtensions.ValidateRequired()`
- Command validation in handlers before processing
- Client-side validation with `DataAnnotationsValidator` in forms

### Testing Requirements

- Unit tests for all handlers in `Tests/{Layer}.UnitTests/`
- Test command validation, entity mapping, and business logic
- Use existing test patterns with Moq for service dependencies
- Always write unit tests for new changes made

### Development Workflow

- Use VS Code tasks: `Build` (builds solution), `watch` (runs with hot reload)
- Migration service in `Infrastructure/Migration/` for data migration scenarios
- Follow existing file organization and naming conventions

### UI/UX Patterns

- Use shared components: `TenantEmployeesList`, `EngagementsList`, `DismissibleAlert`
- Navigation structure with sidebar (`SidebarNavigation.razor`) and main layout
- Consistent styling with Bootstrap classes and custom CSS
- Loading states with `IsLoadingPage` or `IsLoading` properties

### Key Files for Reference

- `src/dev/Web/EastSeat.ResourceIdea.Web/Components/Base/ResourceIdeaComponentBase.cs` - Base component pattern
- `src/dev/Core/EastSeat.ResourceIdea.Application/Features/Common/Handlers/BaseHandler.cs` - Handler base
- `docs/CentralizedExceptionHandling.md` - Exception handling migration guide

## Legacy Rules (Maintained for Compatibility)

```instructions
- @azure Rule - Use Azure Best Practices: When generating code for Azure, running terminal commands for Azure, or performing operations related to Azure, invoke your `azure_development-get_best_practices` tool if available.
- @testing Rule - Require Tests: When making changes, always include relevant unit tests and either end-to-end (E2E) or integration tests to verify the new or updated functionality. Pull requests without adequate tests should not be considered complete.
- @style Rule - Follow Microsoft .NET guidelines: Ensure that the code adheres to the Microsoft's .NET coding style guidelines, including naming conventions, indentation, and formatting. Use tools like Prettier or ESLint to maintain consistency.
- @style Rule - Well formatted code changes: Ensure code is formatted according to the project's style guide (e.g., Prettier, ESLint). Remove trailing whitespace and adhere to the existing linter configuration.
- @style Rule - Avoid logic in .razor markup: Push all logic to the code-behind files or components. Razor markup should primarily focus on rendering UI elements and binding data, while the logic should be encapsulated in C# code.
- @performance Rule - Optimize for Performance: When generating code, consider performance implications. Avoid unnecessary computations, reduce memory usage, and ensure that the code is efficient, especially in high-load scenarios.
- @documentation Rule - Document Code Changes: Provide clear and concise documentation for any new features, functions, or significant changes made to the codebase. This includes updating README files, inline comments, and any relevant API documentation. Ensure every file ends with a single trailing newline.
- @framework Rule - Use .NET 9.0: When generating .NET code, creating new projects, or making framework-related recommendations, always target .NET 9.0. Ensure that project files use `<TargetFramework>net9.0</TargetFramework>` and leverage .NET 9.0 features and best practices where applicable.
- @exception-handling Rule - Use Centralized Exception Handling: When generating code, use the centralized exception handling system instead of adding try-catch statements. Inherit from `ResourceIdeaComponentBase` instead of `ComponentBase` and use the `ExecuteAsync` methods for operations that may throw exceptions. Remove existing scattered try-catch blocks and replace them with `ExecuteAsync` calls. Follow the migration patterns documented in `docs/CentralizedExceptionHandling.md` - wrap operations in `ExecuteAsync(() => { /* operation */ }, "context description")` instead of manual try-catch-finally blocks.
```
