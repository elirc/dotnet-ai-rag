# Security Checklist

| Check | Current Evidence | Status |
| --- | --- | --- |
| Authentication configured | [Program](../../../src/Api/Program.cs#L29-L44) | Implemented |
| Authorization attributes | [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17), [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17) | Implemented for current endpoints |
| Password hashing | [BCryptPasswordHasher](../../../src/Infrastructure/Auth/BCryptPasswordHasher.cs#L5-L10) | Implemented |
| Input validation | [LoginRequest](../../../src/Application/Auth/LoginRequest.cs#L5-L7), [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L9-L12) | Partial |
| Secret management | [appsettings](../../../src/Api/appsettings.json#L5-L9) | Training-only; production risk |
| Token storage | [authStore](../../../frontend/src/app/authStore.ts#L38-L52) | localStorage risk |
| CORS | [Program](../../../src/Api/Program.cs#L17-L23) | Configured for frontend origin |
| CSRF | no cookie auth currently | Not applicable to bearer flow, revisit if cookies added |
| Rate limiting | no evidence found | Missing |
| IDOR/resource ownership | no resource endpoints yet | Future risk |
| SQL injection | EF LINQ query in [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L14-L15) | Low current risk |
| Upload safety | no uploads implemented | Future risk |

## Pre-Merge Security Questions

1. Is the endpoint authenticated?
2. Is the action authorized for this role and resource?
3. Does server-side validation exist?
4. Are secrets outside source-controlled config for production?
5. Does any frontend change increase token exposure?

