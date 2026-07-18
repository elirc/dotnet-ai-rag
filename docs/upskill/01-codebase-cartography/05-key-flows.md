# Key Flows

## Flow: Login And JWT Session

**Why it matters:** This is the first real contract across React, ASP.NET Core, EF Core, hashing, and JWT. If you can trace this, you can trace most future user workflows.

**Open first:** [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L16-L69) - the UI entry point.

| Step | Owner | File | What happens | Data shape | Risk |
| --- | --- | --- | --- | --- | --- |
| 1 | React | [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L21-L35) | Form validates with Zod and submits mutation. | `{ email, password }` | Client validation can be bypassed. |
| 2 | API client | [login](../../../frontend/src/app/api.ts#L29-L33) | Sends JSON to `/auth/login`. | `AuthResponse` expected | Base URL mismatch breaks runtime. |
| 3 | Controller | [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L11-L17) | Allows anonymous login and delegates. | `LoginRequest` | Thin controller depends on service correctness. |
| 4 | Service | [AuthService.LoginAsync](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24) | Normalizes email, checks active user, verifies hash. | `AuthResponse?` | No lockout/rate limiting yet. |
| 5 | Token | [JwtTokenService.CreateToken](../../../src/Infrastructure/Auth/JwtTokenService.cs#L14-L35) | Builds claims and signs JWT. | Bearer token | Token revocation is not modeled. |
| 6 | Browser state | [authStore.setSession](../../../frontend/src/app/authStore.ts#L38-L47) | Stores token and user. | localStorage session | XSS would expose token. |

**Validation & authorization:** [LoginRequest](../../../src/Application/Auth/LoginRequest.cs#L5-L7) validates email shape and required password. [AuthController](../../../src/Api/Controllers/AuthController.cs#L11-L13) explicitly allows anonymous access only for login.

**Persistence & side effects:** [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L14-L17) reads users. No write occurs during login.

**Tests:** Password hash verification has direct unit coverage in [BCryptPasswordHasherTests](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L18). No direct controller/integration login test found.

**Juniors miss:** Client-side validation is not security. **Seniors notice:** rate limiting, lockout, audit logging, refresh/revocation, and secret handling are absent.

**Drill:** Draw the exact data shape after each step. Mark which step first proves the password is correct.

**Self-grade:** Basic = can name files; Solid = can explain claims and hash verification; Strong = can propose a safe integration test and lockout strategy.

## Flow: Protected Dashboard Summary

**Why it matters:** It demonstrates server-side authorization plus client-side UX gating.

**Open first:** [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L68).

| Step | Owner | File | What happens | Data shape | Risk |
| --- | --- | --- | --- | --- | --- |
| 1 | React route | [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18) | Redirects without token. | token/null | UX guard only. |
| 2 | Query | [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L18) | Fetches summary when token exists. | `DashboardSummary` | Token may be expired server-side. |
| 3 | API client | [getDashboardSummary](../../../frontend/src/app/api.ts#L35-L37) | Adds bearer token. | JSON response | Generic error handling only. |
| 4 | Controller | [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17) | Requires authenticated user. | `DashboardSummaryDto` | No role-specific dashboard yet. |
| 5 | Service | [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L5-L37) | Returns placeholder metrics. | static DTO | Metrics can be mistaken for real data. |

**Validation & authorization:** [Program](../../../src/Api/Program.cs#L29-L44) configures JWT validation; [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L10) uses `[Authorize]`.

**Persistence & side effects:** none yet; current service is in-memory placeholder.

**Tests:** No direct dashboard API or UI test found.

**Juniors miss:** `ProtectedRoute` is not a substitute for `[Authorize]`. **Seniors notice:** dashboard metrics need ownership and freshness contracts once backed by real data.

**Drill:** Temporarily imagine localStorage contains a fake token. Which layer rejects it, and why?

**Self-grade:** Basic = explains redirect; Solid = explains bearer validation; Strong = designs a failing-token integration test.

## Flow: Startup Composition And Seeding

**Why it matters:** Startup code is where architecture becomes runtime reality.

**Open first:** [Program](../../../src/Api/Program.cs#L13-L111).

| Step | Owner | File | What happens | Data shape | Risk |
| --- | --- | --- | --- | --- | --- |
| 1 | API | [Program](../../../src/Api/Program.cs#L13-L16) | Adds controllers, Swagger, infrastructure. | DI container | Hidden dependencies surface at startup. |
| 2 | Infrastructure | [AddInfrastructure](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L14-L29) | Registers DbContext and services as scoped. | service graph | Captive dependencies if lifetimes drift. |
| 3 | API | [Program](../../../src/Api/Program.cs#L29-L51) | Configures JWT and policies. | auth handlers | Incorrect policy names fail at runtime. |
| 4 | API | [Program](../../../src/Api/Program.cs#L96-L102) | Middleware order redirects, CORS, auth, controllers. | request pipeline | Wrong order causes confusing auth/CORS failures. |
| 5 | Seeder | [Program](../../../src/Api/Program.cs#L104-L109) | Creates scope and seeds database. | DB migration/users | Startup fails if DB is unavailable. |

**Validation & authorization:** JWT options are read from configuration in [Program](../../../src/Api/Program.cs#L25-L27).

**Persistence & side effects:** [DatabaseSeeder.SeedAsync](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L40) runs migrations and inserts seed users.

**Tests:** No startup integration test found.

**Juniors miss:** Startup seeding is a side effect. **Seniors notice:** automatic migrations at API startup can be risky in multi-instance production deployments.

**Drill:** Explain what breaks first if Postgres is offline. Use the verified Docker failure in the log as evidence.

**Self-grade:** Basic = names middleware order; Solid = explains service scope; Strong = proposes safer migration/seed operational flow.

## Flow: EF User Schema

**Why it matters:** Migrations are the durable contract, not just C# classes.

**Open first:** [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L6-L32).

| Step | Owner | File | What happens | Data shape | Risk |
| --- | --- | --- | --- | --- | --- |
| 1 | Domain | [AppUser](../../../src/Domain/Users/AppUser.cs#L3-L13) | Defines user properties. | C# class | Domain permits any email string. |
| 2 | EF mapping | [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L12-L30) | Maps table, columns, lengths, unique email. | model metadata | Missing normalized-email strategy. |
| 3 | Migration | [InitialIdentity.Up](../../../src/Infrastructure/Persistence/Migrations/20260614190221_InitialIdentity.cs#L14-L36) | Creates `users` table and unique index. | PostgreSQL schema | Case-sensitive email uniqueness is possible. |
| 4 | Seeder | [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L22-L40) | Inserts users if email not present. | rows | Seeder checks original email, login normalizes lower-case. |

**Validation & authorization:** validation belongs at input boundary; persistence enforces size/unique constraints.

**Persistence & side effects:** migration and seeding are persistent side effects.

**Tests:** No migration test found.

**Juniors miss:** `HasIndex(...).IsUnique()` is part of behavior. **Seniors notice:** collation/case sensitivity and normalized lookup strategy matter for identity.

**Drill:** Write a test idea for `Admin@Example.com` vs `admin@example.com`.

**Self-grade:** Basic = reads table shape; Solid = relates model to migration; Strong = identifies case-normalization risk.

## Flow: Password Hash Unit Test

**Why it matters:** Good unit tests isolate a behavior with no database or network.

**Open first:** [BCryptPasswordHasherTests](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L18).

| Step | Owner | File | What happens | Data shape | Risk |
| --- | --- | --- | --- | --- | --- |
| 1 | Test | [BCryptPasswordHasherTests](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L8-L17) | Hashes password and verifies correct/wrong input. | bool assertions | Does not test AuthService or JWT. |
| 2 | Impl | [BCryptPasswordHasher](../../../src/Infrastructure/Auth/BCryptPasswordHasher.cs#L5-L10) | Delegates to BCrypt.Net. | string hash | Cost factor is default and undocumented. |

**Validation & authorization:** none; this is cryptographic utility behavior.

**Persistence & side effects:** none durable.

**Tests:** Direct coverage exists.

**Juniors miss:** a hash is intentionally different each time due to salt. **Seniors notice:** the test checks behavior, not exact hash text.

**Drill:** Explain why asserting the hash string equals a fixed value would be wrong.

**Self-grade:** Basic = runs test; Solid = explains salt; Strong = designs AuthService tests using a fake hasher or test database.

