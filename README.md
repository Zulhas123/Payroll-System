# Payroll System (ASP.NET Core MVC + EF Core)

Minimal payroll management app using:
- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server
- Repository pattern (simple EF-based repository)

## Features (implemented)
- Departments: CRUD
- Employees: CRUD (department + basic salary + active flag)
- Payroll runs: generate a run for a pay period for all active employees (basic salary only) and view results

## Architecture (Clean Architecture folder layout)
Projects are organized under `src/`:
- `src/Domain/PayrollSystem.Domain` — entities
- `src/Application/PayrollSystem.Application` — abstractions (interfaces)
- `src/Infrastructure/PayrollSystem.Infrastructure` — EF Core persistence + repository implementations
- `src/Web/PayrollSystem.Web` — ASP.NET Core MVC UI

## Setup

### 1) Configure SQL Server connection
Edit `src/Web/PayrollSystem.Web/appsettings.json` (`ConnectionStrings:DefaultConnection`) to match your SQL Server.

Default (LocalDB):
`Server=(localdb)\\MSSQLLocalDB;Database=PayrollSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true`

### 2) Create database (Code First)
Install EF CLI tool (choose one):
- Local tool (recommended): `dotnet tool install dotnet-ef --tool-path .\\.tools`
- Global tool: `dotnet tool install -g dotnet-ef`

Then from repo root:
- `dotnet ef migrations add InitialCreate --project .\\PayrollSystem.Web --startup-project .\\PayrollSystem.Web`
- `dotnet ef database update --project .\\PayrollSystem.Web --startup-project .\\PayrollSystem.Web`

### 3) Run
- `dotnet run --project .\\src\\Web\\PayrollSystem.Web`
