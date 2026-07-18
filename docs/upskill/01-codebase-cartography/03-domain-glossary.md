# Domain Glossary

| Term | Meaning Here | Evidence | Transferable Concept |
| --- | --- | --- | --- |
| User | Authenticated human with email, display name, password hash, role, and active flag | [AppUser](../../../src/Domain/Users/AppUser.cs#L3-L13) | Identity object and authorization subject. |
| Role | Coarse permission group such as Admin or Engineer | [Program policies](../../../src/Api/Program.cs#L46-L51) | RBAC is simple but can become too coarse for resource ownership. |
| JWT | Bearer token containing identity and role claims | [JwtTokenService](../../../src/Infrastructure/Auth/JwtTokenService.cs#L17-L35) | Stateless auth shifts revocation/session concerns elsewhere. |
| Dashboard Summary | DTO returned to the UI for operational metrics | [DashboardSummaryDto](../../../src/Application/Dashboard/DashboardSummaryDto.cs#L3-L18) | Read models can differ from domain entities. |
| Seed User | Startup-created account for local learning | [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L14-L40) | Seed data should be idempotent and environment-aware. |
| Protected Route | Client-side redirect when no token is present | [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18) | UX guard, not an authorization boundary. |

