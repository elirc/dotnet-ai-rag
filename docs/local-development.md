# Local Development

## Prerequisites

- Docker Desktop
- Node.js 22+
- .NET SDK 8. The SDK was found locally at `C:\Users\Owner\.dotnet\dotnet.exe`.

## Start PostgreSQL

```powershell
docker compose up -d postgres
```

## Run Backend

```powershell
& 'C:\Users\Owner\.dotnet\dotnet.exe' restore
& 'C:\Users\Owner\.dotnet\dotnet.exe' run --project src/Api
```

The API applies EF migrations and seeds users on startup when `SeedDatabaseOnStartup` is `true`.

For production-like environments, run migrations as a release step instead of API startup. See `docs/adr/0004-startup-migrations.md`.

## Run Frontend

```powershell
cd frontend
npm install
npm run dev
```

The frontend expects the API at `https://localhost:7223/api` by default. Override with `VITE_API_BASE_URL` if the API runs elsewhere.

## Full Compose Profile

```powershell
docker compose --profile app up --build
```

This builds and runs the API and frontend containers in addition to Postgres. The compose frontend is static and points at whatever API base URL was baked into the build, so local Vite remains the preferred development loop.

## Useful Commands

```powershell
& 'C:\Users\Owner\.dotnet\dotnet.exe' ef migrations add Name --project src/Infrastructure/NetAiOps.Infrastructure.csproj --startup-project src/Api/NetAiOps.Api.csproj --output-dir Persistence/Migrations
& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln
cd frontend; npm run build
```
