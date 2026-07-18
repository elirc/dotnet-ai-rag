# Command Cheatsheet

| Command | Status | Evidence |
| --- | --- | --- |
| `rg --files -g '!frontend/node_modules' -g '!frontend/dist' -g '!**/bin/**' -g '!**/obj/**'` | Verified pass | Used for inventory. |
| `git rev-parse HEAD` | Verified fail | Unborn branch; no HEAD. |
| `git log --oneline -30` | Verified fail | No commits on branch. |
| `& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln` | Verified pass | 1 test passed. |
| `cd frontend; npm run build` | Verified pass | TypeScript and Vite build passed. |
| `docker compose up -d` | Verified fail | Docker daemon unavailable. |
| `& 'C:\Users\Owner\.dotnet\dotnet.exe' run --project src/Api` | Inferred | Expected from README, not run in this pass because DB unavailable. |
| `cd frontend; npm run dev` | Inferred | Expected from package script, not run in this pass. |

See [verification log](verification-log.md) for trimmed outputs.

