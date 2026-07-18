# Command Cheatsheet

```powershell
# Provenance
git rev-parse HEAD
git log --oneline -30

# Backend
& 'C:\Users\Owner\.dotnet\dotnet.exe' restore
& 'C:\Users\Owner\.dotnet\dotnet.exe' build NetAiOps.sln
& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln
& 'C:\Users\Owner\.dotnet\dotnet.exe' run --project src/Api

# Frontend
cd frontend
npm install
npm run dev
npm run build

# Docker
docker compose config
docker compose up -d postgres
docker compose --profile app up --build
```

Source setup commands are documented in [README](../../README.md#L34-L67) and [docker-compose.yml](../../docker-compose.yml#L1-L47).
