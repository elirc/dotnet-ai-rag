# Risk Register

| Risk | Evidence | Impact | Likelihood | Suggested Test | Suggested Fix | Confidence |
| --- | --- | --- | --- | --- | --- | --- |
| API startup fails when DB unavailable | [Program seeding](../../../src/Api/Program.cs#L104-L109), [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L11), verified Docker failure | App cannot start | High in local env without Docker | Start API with DB down | Dev-only seeding flag; separate migration job | High |
| JWT signing key stored in appsettings | [appsettings](../../../src/Api/appsettings.json#L5-L9) | Secret leakage in real deployment | Medium if copied to prod | Config validation test | Environment secret provider | High |
| No auth endpoint integration tests | Only [BCryptPasswordHasherTests](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L18) found | Regressions in login/authz | Medium | WebApplicationFactory tests | Add integration test project | High |
| localStorage bearer token exposure | [authStore](../../../frontend/src/app/authStore.ts#L38-L52) | XSS can steal token | Medium | Security review / XSS test where applicable | Harden XSS, consider cookies | High |
| Placeholder dashboard appears real | [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L25-L35) | Misleads learners/users | Medium | UI assertion for mock label | Label metrics as mock until backed by DB | High |
| Case-sensitive email uniqueness | [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L15-L20), [AuthService normalizes](../../../src/Infrastructure/Auth/AuthService.cs#L14-L15) | Duplicate identity confusion | Medium | Insert mixed-case emails | Normalize stored email or use citext/index | Medium |
| No rate limiting/lockout | [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24) has no lockout evidence | Brute-force risk | Medium | Repeated invalid login test | Rate limiter + audit events | High |
| No correlation ID/error middleware | [Program pipeline](../../../src/Api/Program.cs#L88-L102) | Harder debugging | Medium | Failing request log test | Add middleware and structured errors | High |

