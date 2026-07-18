# Contributing

This repository is a training-oriented internal operations platform. Treat changes as if they were going through a mid-size company review.

## Local Checks

```powershell
& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln
cd frontend
npm run build
```

## Expectations

- Keep controllers thin and put behavior behind application service contracts.
- Add server-side authorization for every protected API.
- Bound list endpoints with pagination.
- Add or update migrations when persistence changes.
- Update docs when a feature changes architecture or local setup.
- Include rollback notes for risky migrations or auth changes.
