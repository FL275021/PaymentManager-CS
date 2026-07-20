# Payment Manager

A system for managing payments made by company teams. Each user registers payments linked to their team, with role-based access controlling what they can see and do.

## Roles

| Role | Can do |
|---|---|
| **Employee** | Login/logout, reset password, register a payment, view own payments |
| **Manager** | Everything an Employee can, plus: view monthly payments, list users exceeding a spending threshold, run audits |
| **Admin** | Everything above, plus: manage users, payments, expense types, and teams |

## Architecture

The solution is split into two independently deployable ASP.NET Core apps, both targeting .NET 8:

- **`Backend-API`** — a REST API following Clean Architecture / SOLID, split into `LogicaNegocio` (domain), `LogicaAplicacion` (use cases), `LogicaAccesoDatos` (EF Core data access), and `WebApi` (controllers). Issues and validates JWT bearer tokens.
- **`Frontend-MVC-Client`** — an ASP.NET Core MVC client that consumes the API and also talks to the same SQL Server database directly via EF Core for some read paths.

Both projects are deployed independently to Azure (App Service + Azure SQL).

## Tech stack

- ASP.NET Core 8 (Web API + MVC)
- Entity Framework Core
- SQL Server (Azure SQL Database)
- JWT Bearer authentication
- Bootstrap / jQuery (MVC front end)

## Getting started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- A SQL Server instance (local or Azure SQL)

### Configure secrets

Neither `appsettings.json` file ships with real credentials. Set the following via `dotnet user-secrets`, environment variables, or a local (gitignored) `appsettings.Development.json` — never commit real values:

**`Project/Backend-API/WebApi`**
```json
{
  "ConnectionStrings": { "MiConexionLocal": "<your SQL Server connection string>" },
  "SecretTokenKey": "<a random secret used to sign JWTs>"
}
```

**`Project/Frontend-MVC-Client/WebMVC`**
```json
{
  "ConnectionStrings": {
    "MiConexionLocal": "<your SQL Server connection string>",
    "URLApiUsuarios": "<Backend-API base URL>/api/Usuario",
    "URLApiLogin": "<Backend-API base URL>/api/Usuario/login",
    "URLApiPagos": "<Backend-API base URL>/api/Pago",
    "URLApiEquipos": "<Backend-API base URL>/api/Equipo",
    "URLApiGastos": "<Backend-API base URL>/api/Gasto"
  }
}
```

Example using user-secrets from each project folder:
```bash
cd Project/Backend-API/WebApi
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:MiConexionLocal" "<connection string>"
dotnet user-secrets set "SecretTokenKey" "<random secret>"
```

### Run

Apply EF Core migrations, then run each project from its own folder:

```bash
cd Project/Backend-API/WebApi
dotnet ef database update
dotnet run

# in a second terminal
cd Project/Frontend-MVC-Client/WebMVC
dotnet run
```

The MVC client's `wwwroot/lib` (Bootstrap, jQuery) is restored via LibMan and is not committed — run `libman restore` if it's missing.

## Screenshots

<img width="1595" height="721" alt="Login" src="https://github.com/user-attachments/assets/4d6a652f-b75e-4e16-81f9-c49cbce195c9" />
<img width="1597" height="725" alt="My Payments" src="https://github.com/user-attachments/assets/d07664b0-78ad-4190-a831-bfd163350943" />
<img width="1589" height="733" alt="Create Payment" src="https://github.com/user-attachments/assets/7329a604-ddc1-48df-9ce6-2fd231e265d6" />
<img width="1599" height="731" alt="Payment List" src="https://github.com/user-attachments/assets/8780ebc5-dda2-42e9-95c9-e06d923a95e5" />
<img width="1595" height="733" alt="Monthly Payment List" src="https://github.com/user-attachments/assets/b8634f50-b253-42db-aa87-3a0d28afe6e8" />
<img width="1594" height="723" alt="Edit Recurring Payment" src="https://github.com/user-attachments/assets/12c26cfc-cc00-41ff-8814-f767be3f9d1b" />
<img width="1590" height="729" alt="Edit One-off Payment" src="https://github.com/user-attachments/assets/8552b27e-6ae1-46ce-8ff4-a52368816204" />
<img width="1598" height="729" alt="Payment Detail" src="https://github.com/user-attachments/assets/1a62a5fa-eff8-4a09-a2f0-2575f55f0761" />
