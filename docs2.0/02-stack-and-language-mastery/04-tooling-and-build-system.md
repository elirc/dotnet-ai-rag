# Tooling And Build System

Verified commands during this pass:

```powershell
& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln
cd frontend
npm run build
docker compose config
```

Backend CI uses .NET 8, restores the solution, and runs tests in [ci.yml](../../.github/workflows/ci.yml#L13-L24). Frontend CI uses Node 22, `npm ci`, and `npm run build` in [ci.yml](../../.github/workflows/ci.yml#L16-L30).

Local Docker is described by [docker-compose.yml](../../docker-compose.yml#L1-L47). The default profile starts Postgres; the `app` profile also builds API and frontend services. During this pass only compose configuration was validated, not container startup.

Contribution expectations are documented in [CONTRIBUTING](../../CONTRIBUTING.md#L5-L20): test backend, build frontend, keep controllers thin, protect APIs, bound list endpoints, add migrations, update docs, and include rollback notes for risky changes.
