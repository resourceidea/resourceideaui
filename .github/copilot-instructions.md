## EastSeat.ResourceIdea – Copilot Contribution & Architecture Guide

### 1. Purpose

This repository hosts a multi–layer ASP.NET Blazor Server SaaS for resource planning in an audit & tax advisory context (Uganda). Copilot and contributors MUST preserve simplicity, clarity, testability, and avoid over–engineering.

### 2. High-Level Architecture (No MediatR / No Paid 3rd Party Frameworks)

Solution (to be added) `EastSeat.ResourceIdea.slnx` will contain:

- Domain: Entities, Value Objects, Enums, Guard/Validation helpers, Static Domain Services (pure functions).
- Application: Application Service classes (method-per-use-case). Optional lightweight Command / Query DTOs; NO pipelines, NO MediatR.
- Infrastructure: EF Core (PostgreSQL via Npgsql), DbContext, EntityTypeConfigurations, Identity integration, Repository _optional_ (DbContext can be used directly).
- Server: Blazor Server project (MudBlazor for UI), Razor Pages/Components, DI wiring, Identity UI (custom components – no scaffold).
- Tests: xUnit projects (Domain, Application, Integration + Testcontainers later, bUnit for UI when added).

### 3. Technology Choices

- .NET 9, C# (functional style with LINQ where sensible, keep readability).
- Database: PostgreSQL; migrations via EF Core.
- Authentication & Authorization: ASP.NET Identity (custom User / Role classes), cookie auth.
- UI Styling: MudBlazor (OSS, acceptable). No commercial UI libraries.
- Logging: `ILogger<T>` (Serilog optional later – if added must be justified in PR description).
- Configuration: appsettings + environment variables (.env file ignored; `.env.sample` tracked).
- Use .slnx solution format only for faster load times. DO NOT create .sln files.

### 4. Prohibited / Restricted

- DO NOT introduce MediatR (ignored by Dependabot intentionally) or other CQRS bus frameworks.
- Avoid paid or proprietary libraries. If proposing a new dependency: MUST justify (security, performance, maintainability) and prefer Microsoft / OSS minimal libs.
- No automatic code generation that obfuscates logic.

### 5. Coding Conventions

- Nullable enabled; treat warnings seriously – prefer explicit null checks or guard clauses.
- Use explicit access modifiers; avoid `public` unless required.
- Favor small immutable records/value objects for strongly typed IDs and small concepts.
- Keep entity constructors internal/private; supply static `Create` methods enforcing invariants.
- Do not overuse inheritance – prefer composition.
- LINQ: readable; break long chains into local variables.
- Avoid premature async across entire stack; use async only for I/O operations.
- Keep methods under ~40 lines; extract intentful private helpers when needed.

### 6. Domain Guidelines (Examples)

Entities expected: `Client`, `Engagement`, `EngagementYear`, `Employee`, `Assignment`, `LeaveRequest`, `Holiday`, `PublicHoliday` (may merge with Holiday), `RolloverHistory`. Enumerations (simple enums) for: EngagementType, EngagementStatus, EmploymentStatus, LeaveType, AllocationType.
Rules:

- Assignment must not cause >100% allocation overlap for an employee per working day.
- Leave overrides assignment availability (planner must show leave coloration).
- Weekends (Sat/Sun) & Public Holidays are non-working – centralize logic in a `CalendarService`.
- Rollover duplicates EngagementYear + Assignments with leap-year adjustments (Feb 29 -> Feb 28 if next year non-leap).

### 7. Application Services Pattern

Create classes like `EngagementService`, `AssignmentService`, `PlannerService`, `RolloverService` exposed via DI. Each public method = atomic use case, performing:

1. Validation / Guard clauses.
2. Query (read model projection for queries) or modification (attach/update entities).
3. Persistence: `await dbContext.SaveChangesAsync()`.
4. Return DTO / Result object.
   No mediator, no pipeline behaviors. Cross-cutting concerns (logging, caching) done inline or via simple decorator interfaces.

### 8. Identity & Authorization

- Custom `ApplicationUser : IdentityUser<Guid>` with extra profile fields (DisplayName, StaffCode, DailyCapacityHours).
- Custom `ApplicationRole : IdentityRole<Guid>` with optional Seniority / Rate.
- Policies: e.g. `PolicyNames.ManageEngagements`, `PolicyNames.ViewPlanner`.
- Seed Admin + Roles on startup (conditional by environment variable).

### 9. Resource Planner UI Principles

- Horizontal year view (1–12 months, days virtualization) Rows = Employees.
- Color legend for: Engagement, Leave Types, Weekend, Public Holiday.
- Merge contiguous identical segments to reduce DOM elements.
- Use MudBlazor Grid / Table with sticky left column; consider row virtualization if employee count large.

### 10. Testing Strategy (xUnit)

- Domain tests: pure logic (allocation overlap, leave working-day calculation, rollover date mapping).
- Application service tests: use real EF Core with InMemory _only for logic not relying on provider specifics_ or PostgreSQL Testcontainer (preferred later for date/SQL characteristics).
- Integration tests: multi-entity scenarios (create engagement, assignments, rollover).
- UI tests (later): bUnit for components (Planner row rendering, identity forms).
- Snapshot tests: Planner HTML segments (store in `tests/Snapshots/` – ensure `.snap` not ignored).
- Keep tests deterministic; freeze DateTime via abstraction (e.g. `ITimeProvider`).

### 11. Performance & Data Access

- Prefer projection (`Select`) to DTO rather than materializing large entity graphs for read models.
- Cache year planner data by (Year, EmployeeSetHash) with invalidation on assignment/leave changes.
- Avoid N+1 queries: use `Include` only when necessary; evaluate splitting large queries into purposeful projections.

### 12. Configuration & Secrets

- `appsettings.*.json` for stable config; secrets via environment variables / Azure Key Vault in production.
- `.env.sample` maintained; `.env` ignored.
- Do not commit real connection strings or credentials.

### 13. CI / Workflows Expectations

- Build & Test workflows reference `EastSeat.ResourceIdea.slnx` once added.
- CodeQL runs weekly + on PR for security & quality.
- Dependabot weekly NuGet updates; custom ignore list enforced.
- Each PR must include: Description (What / Why), Architecture impact, Test coverage notes, Manual verification steps.

### 14. Pull Request Review Checklist (For Humans & Copilot)

1. Clear purpose & linked issue/user story.
2. No forbidden dependencies (MediatR, paid libs).
3. Domain invariants preserved; new invariants documented.
4. Tests added/updated; all pass locally.
5. No secrets or credentials.
6. Public API changes documented in README or separate migration note if needed.
7. Code style consistent (nullable, naming conventions, minimal warnings).

### 15. Logging & Observability (Future Enhancements)

- Start: basic `ILogger` usage.
- Later: add Serilog + Application Insights if needed (open a proposal PR first).
- Important events: Rollover executed, Assignment overlap rejection, Leave approval.

### 16. Migration & Database

- Migrations belong in Infrastructure project under `Migrations/` folder.
- Use explicit naming: `AddEngagementYearTable`, `AddAssignmentIndexes`.
- Never edit generated migration classes except to add data seeding needed for structural integrity (not business seed).

### 17. Style & Naming

- Projects: `EastSeat.ResourceIdea.[Layer]`.
- Namespaces match folder structure.
- DTO suffix optional; use descriptive names (`EngagementSummary`, `AssignmentSegment`).
- Avoid abbreviations unless industry standard.

### 18. Future Multi-Tenancy (Planning Hook)

- Prepare entities to add a `TenantId` (GUID) column later; do not add until requirement confirmed.
- Keep queries central; potential global filter can be applied later.

### 19. Rollover Edge Cases

- Leap year mapping; log adjustments.
- Exclude assignments for terminated employees.
- Maintain allocation percentages.
- Provide dry-run preview summarizing counts & skips.

### 20. Contribution Flow (Simplified)

Fork/branch -> changes -> ensure tests -> update docs if needed -> PR with checklist.

### 21. AI Assistant Guidance

- Provide smallest viable change sets.
- When generating code: include comments only where clarifying domain rules.
- Suggest test cases with each non-trivial domain change.
- Never introduce disallowed dependencies.

### 22. Security Considerations

- Validate all user-modifiable date ranges (start <= end).
- Enforce authorization on all state-changing methods.
- Sanitize user-visible strings (future: add content sanitization if rich text introduced).

### 23. What NOT to Do

- Do not merge partially failing tests.
- Do not rely on reflection-based magic to wire logic.
- Do not embed business logic in Razor markup directly; keep it in services.

### 24. Maintenance & Evolution
- Regularly review dependencies for updates/security.
- Archive stale branches after merge.
- Update this guide as architecture evolves; keep it relevant.
- Update `AppStartup.md` with any changes to startup/run instructions.
- Update `README.md` with any new environment variables or configuration changes.
- Update `ONBOARDING.md` when changes are made that affect new developer setup.

---

This document evolves; propose edits via PR when architecture or constraints change.
