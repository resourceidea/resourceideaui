# EastSeat.ResourceIdea

First release (v1) of EastSeat.ResourceIdea: a Blazor WebAssembly + ASP.NET Core minimal API application for managing allocation and assignment of staff to client engagements. This version focuses on authentication and Client CRUD. Future iterations will introduce richer resource planning, engagements, scheduling, and domain/application layering (CQRS + MediatR) once complexity warrants it.

## Tech Stack

- .NET 9.0 (current). Multi-target (net10.0) will be introduced when stable SDK/CI support is available.
- Blazor WebAssembly (Client)
- ASP.NET Core Minimal APIs (Server)
- Entity Framework Core + PostgreSQL (Npgsql) (InMemory used for unit tests)
- ASP.NET Core Identity + JWT auth
- MudBlazor UI library
- Docker (Linux images; nginx for static client hosting)

## Solution & Project Naming

All projects and namespaces are prefixed with `EastSeat.ResourceIdea`:

- `EastSeat.ResourceIdea.slnx` (solution)
- `src/Shared/EastSeat.ResourceIdea.Shared.csproj` (models & DTOs)
- `src/Server/EastSeat.ResourceIdea.Server.csproj` (minimal API + Identity + JWT)
- `src/Client/EastSeat.ResourceIdea.Client.csproj` (Blazor WASM UI)
- `tests/ServerTests/EastSeat.ServerTests.csproj` (early unit tests)

The solution uses the modern `.slnx` format.

## Current Features (v1)

- User registration & login (JWT issued by server)
- Password reset (token issued; email delivery simulated via console/log for now)
- Environment-variable driven user & role seeding from JSON (no hard‑coded accounts)
- Client entity CRUD (Name, Code, Status) with role-based authorization (Admin/Manager for write operations)
- Blazor pages: Home, Login, Register, Forgot/Reset Password, Profile (shows token), Clients list & create/edit
- Docker compose setup (Postgres + Server + Client) with startup scripts (`start.ps1` / `start.sh`)
- Unit tests: Client CRUD + user/role seeding

## Project Structure

```text
src/
  Shared/        # Shared POCO models & DTOs
  Server/        # Minimal API, Identity, EF Core, seeding
  Client/        # Blazor WASM (MudBlazor UI)
tests/
  ServerTests/   # xUnit tests (CRUD + seeding)
config/seeds/    # JSON seed user definitions
docker/          # Dockerfiles & compose
scripts/         # Helper scripts
```

## Getting Started (Local Development)

1. Install .NET 9 SDK (`dotnet --version`).
2. Copy `.env.example` to `.env` and update secrets & configuration.
3. Build the solution:

```powershell
dotnet build EastSeat.ResourceIdea.slnx
```

4. Run the Server (minimal API):

```powershell
dotnet run --project src/Server/EastSeat.ResourceIdea.Server.csproj
```

5. Run the Client (Blazor WASM):

```powershell
dotnet run --project src/Client/EastSeat.ResourceIdea.Client.csproj
```

Server default dev URL: `https://localhost:5001`  
Client dev URL (Blazor dev server): `https://localhost:5002` (adjust if port differs). In Docker, client is served via nginx on `http://localhost:8080`.

## Docker Usage

1. Ensure `.env` exists (copied from `.env.example`).
2. Start (optionally seeding) via PowerShell:

```powershell
./start.ps1 -Seed
```

Or on Linux/macOS:

```bash
./start.sh
```

Services provisioned:

- `postgres` (PostgreSQL 16-alpine)
- `server` (ASP.NET Core minimal API on 5001)
- `client` (nginx static host on 8080)

Seeding runs only when `RESOURCEIDEA_RUN_SEED=true`. Users & roles are loaded from the JSON file indicated by `RESOURCEIDEA_SEED_USERS_FILE` using the password in `RESOURCEIDEA_DEFAULT_PASSWORD`.

## Environment Variables (.env)

| Name | Purpose |
|------|---------|
| POSTGRES_HOST | Database host (container: postgres) |
| POSTGRES_DB | Database name |
| POSTGRES_USER | DB username |
| POSTGRES_PASSWORD | DB password (change for production) |
| JWT_ISSUER | JWT issuer |
| JWT_AUDIENCE | JWT audience |
| JWT_KEY | Symmetric key (rotate regularly) |
| RESOURCEIDEA_RUN_SEED | Enable user/role seeding when `true` |
| RESOURCEIDEA_SEED_USERS_FILE | Path to JSON seed file |
| RESOURCEIDEA_DEFAULT_PASSWORD | Default password for seeded accounts |

## Sample Seed Users & Roles

`config/seeds/sample-users.json` includes sample accounts:

- `admin@example.com`  → Admin
- `manager@example.com` → Manager
- `staff1@example.com`, `staff2@example.com` → Staff

Adjust roles/users as needed in your own seed file. Never commit real passwords.

## Testing

Run the full test suite:

```powershell
dotnet test EastSeat.ResourceIdea.slnx --no-build
```

Or target just server tests:

```powershell
dotnet test tests/ServerTests/EastSeat.ServerTests.csproj --no-build
```

Included tests:

- EF Core InMemory Client CRUD
- User & role seeding (JSON-driven)

## Seeding Notes

User seeding occurs at startup when enabled via environment configuration. The process:

1. Reads JSON entries (email + roles).
2. Ensures roles exist.
3. Creates users (assigning `Email` as `UserName`).
4. Adds missing roles.

Seeding logic is intentionally simple for v1; future iterations may enforce stronger validation and audit logging.

## Roadmap (Future Iterations)

- Domain & Application layers (CQRS with MediatR)
- Engagement & allocation business rules
- Centralized client component base for unified loading/error UX
- Integration tests (Testcontainers + PostgreSQL)
- Email provider integration for password reset & notifications
- Role/permission granularity expansion

## Contributing

1. Fork and clone the repository.
2. Create a feature branch.
3. Implement changes with accompanying tests.
4. Submit a PR (CI will build, test, and build docker images).

## Security Notes

- Do NOT commit real secrets; replace JWT key & DB password outside version control for production.
- Rotate JWT signing keys regularly.
- Review Docker & environment settings before deployment.

## License

TBD.

