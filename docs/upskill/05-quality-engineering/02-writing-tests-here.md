# Writing Tests Here

## Recipes

| Recipe | Target | Command | Acceptance |
| --- | --- | --- | --- |
| Password mismatch | [BCryptPasswordHasher](../../../src/Infrastructure/Auth/BCryptPasswordHasher.cs#L5-L10) | `dotnet test --filter BCryptPasswordHasherTests` | wrong password false |
| Login happy path | [AuthController](../../../src/Api/Controllers/AuthController.cs#L11-L17) | inferred: WebApplicationFactory test | returns token and user |
| Invalid login | [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L17-L20) | inferred | returns 401/null |
| Dashboard unauthenticated | [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17) | inferred | returns 401 |
| Admin users authorized | [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17) | inferred | Admin 200, Viewer 403 |
| Migration shape | [InitialIdentity](../../../src/Infrastructure/Persistence/Migrations/20260614190221_InitialIdentity.cs#L14-L36) | inferred | users table + unique email |
| UI login error | [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L56-L59) | inferred: React Testing Library | error rendered |

## Verified Test Command

```powershell
& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln
```

Result: passed; see [verification log](../08-reference/verification-log.md).

