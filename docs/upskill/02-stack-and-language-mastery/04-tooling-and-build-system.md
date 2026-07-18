# Tooling And Build System

## .NET

The solution is `NetAiOps.sln`. Verified test command:

```powershell
& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln
```

The API project references authentication, EF design tooling, Swagger, and layered projects in [NetAiOps.Api.csproj](../../../src/Api/NetAiOps.Api.csproj#L9-L23).

## Frontend

Frontend scripts are in [package.json](../../../frontend/package.json#L6-L10). Verified build command:

```powershell
cd frontend
npm run build
```

## Docker

[docker-compose.yml](../../../docker-compose.yml#L1-L20) defines PostgreSQL, but `docker compose up -d` failed in this environment because Docker daemon was unavailable. Do not mark database startup verified until that command passes locally.

## Drill

When a build fails, classify it: restore/package problem, compile/type problem, runtime config problem, or external service problem. Use the Docker failure as the external-service example.

