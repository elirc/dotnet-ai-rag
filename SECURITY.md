# Security Policy

## Reporting

This is a local training project. In a real organization, report suspected vulnerabilities privately through the security team or repository security advisory process.

## Baseline Checks

- Do not commit production secrets.
- Keep JWT signing keys in environment/secret storage outside source control.
- Require authorization attributes on protected endpoints.
- Use human approvals before agent-triggered risky actions.
- Add tests for auth boundaries and resource ownership.

## Where the code stands against the baseline (verified 2026-10-06)

The checks above are goals. This table records what the code actually does today, so
nobody mistakes a goal for a protection. Every row was confirmed by reading the cited
source; nothing was exploited or run.

| Baseline item | Current state | Evidence |
| --- | --- | --- |
| No production secrets committed | Only dev values are committed, but the dev JWT signing key is committed in **three** places, and startup accepts it. `Program.cs` rejects keys shorter than 32 characters, and the placeholder is longer than that, so a deployment that forgets to override `Jwt__SigningKey` runs with a publicly known HS256 key. Anyone can then mint an `Admin` token. | [`appsettings.json:8`](src/Api/appsettings.json), [`docker-compose.yml:29`](docker-compose.yml), [`.env.example:7`](.env.example), [`Program.cs:41-50`](src/Api/Program.cs#L41-L50) |
| Signing key outside source control | Not enforced: there is no check for the placeholder value and no environment-specific failure. *Fix direction:* refuse to start outside Development when the key equals the committed placeholder. | same as above |
| Authorization attributes on protected endpoints | **Met at the controller level.** Every controller has `[Authorize]` or a policy; only `POST /api/auth/login` (`[AllowAnonymous]`, rate-limited 10/min/IP) and the two health checks are anonymous. | `src/Api/Controllers/*.cs`; [`Program.cs:19-31`](src/Api/Program.cs#L19-L31), [`Program.cs:129-130`](src/Api/Program.cs#L129-L130) |
| …and on data, not just endpoints | **Partial.** Tickets have an object check (`CanReadTicket`), but any authenticated user, including `Viewer`, can read *all* logs, incidents, and knowledge documents, and a ticket's assignee sees its internal notes whatever their role. | [`OperationsService.cs:224-228`](src/Infrastructure/Operations/OperationsService.cs#L224-L228), [`439-475`](src/Infrastructure/Operations/OperationsService.cs#L439-L475), [`517-520`](src/Infrastructure/Operations/OperationsService.cs#L517-L520), [`601`](src/Infrastructure/Operations/OperationsService.cs#L601) |
| Human approval before risky agent actions | **Approval rows are created, but no decision path exists.** The investigation saves a `Pending` `HumanApproval`; the only approvals endpoint is a `GET` list, and `HumanApproval` has no link to the run or incident it approves. Nothing risky executes today, so nothing is currently unguarded. But "approval" is a record, not a control yet. | [`OperationsService.cs:419-427`](src/Infrastructure/Operations/OperationsService.cs#L419-L427), [`ApprovalsController.cs`](src/Api/Controllers/ApprovalsController.cs), [`HumanApproval.cs`](src/Domain/Operations/HumanApproval.cs) |
| Tests for auth boundaries and ownership | **Partial.** Role → capability mapping (4 cases) and "Viewer cannot run an investigation" are tested. No test covers ticket ownership, internal-note visibility, or any HTTP-level policy. | [`RequestContextTests.cs`](tests/UnitTests/RequestContextTests.cs), [`OperationsServiceTests.cs:110-138`](tests/UnitTests/OperationsServiceTests.cs#L110-L138) |

Other things a reviewer should know:

- **Login** has no account lockout. It skips the BCrypt verify when the email is unknown,
  which makes a timing difference between "no such user" and "wrong password"
  ([`AuthService.cs:15-20`](src/Infrastructure/Auth/AuthService.cs#L15-L20)).
- **Tokens** are HS256, last 120 minutes (`Jwt:ExpiresMinutes`), and can't be revoked or
  refreshed. The frontend keeps the session in `localStorage`
  ([`authStore.ts`](frontend/src/app/authStore.ts)), so an XSS bug is a token-theft bug.
- **Errors**: unexpected exceptions return a generic 500 message; `UnauthorizedAccessException`
  → 403 and `InvalidOperationException` → 400 return the exception message to the client
  ([`ExceptionHandlingMiddleware.cs:16-41`](src/Api/Middleware/ExceptionHandlingMiddleware.cs#L16-L41)).
- **Headers**: `X-Content-Type-Options: nosniff`, `X-Frame-Options: DENY`, and
  `Referrer-Policy: no-referrer` are set on every response
  ([`CorrelationAndSecurityHeadersMiddleware.cs:13-17`](src/Api/Middleware/CorrelationAndSecurityHeadersMiddleware.cs#L13-L17)).
  The `X-Correlation-ID` request header is echoed and logged without validation.
- **Seed users** all share the password `Password123!`, and seeding runs on startup by
  default (`SeedDatabaseOnStartup: true`).

For a guided walk through the investigation workflow's own trust boundaries, see
[`astraupskill/`](astraupskill/README.md) chapters 1 and 5-6, and extended lessons 8, 12,
and 17.
