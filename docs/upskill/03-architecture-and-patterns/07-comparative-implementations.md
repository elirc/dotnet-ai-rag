# Comparative Implementations

## Comparison 1: Controllers vs Minimal APIs

**The decision this repo made:** MVC controllers expose HTTP endpoints - [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L7-L18).

**The alternative:** minimal APIs colocate route mapping and handler delegates.

```csharp
// Illustrative: not from this repo
app.MapPost("/api/auth/login", async (LoginRequest request, IAuthService auth, CancellationToken ct) =>
    await auth.LoginAsync(request, ct) is { } response ? Results.Ok(response) : Results.Unauthorized());
```

| Dimension | This repo's choice | Alternative |
| --- | --- | --- |
| Complexity / ceremony | More files and attributes | Less ceremony for tiny endpoints |
| Testability | Familiar controller integration tests | Handler testing can be direct but route sprawl is possible |
| Performance characteristics | MVC overhead is negligible here | Minimal APIs can be leaner |
| Coupling & blast radius | Controllers organize by resource | Routes can concentrate in Program if undisciplined |
| Failure modes unique to it | Fat controllers | Giant route file |
| Migration cost | Low now, higher after many controllers | Easy before Phase 2 |

**What each choice commits you to / who pays:** Controllers commit maintainers to resource-oriented files and attributes. Minimal APIs commit maintainers to route organization discipline.

**Verdict for this repo:** Right call. The project is meant to teach enterprise ASP.NET Core, and controllers make auth attributes visible.

**Switch signal:** revisit if endpoints remain tiny and controller files become empty pass-through wrappers.

**Drill:** Convert `DashboardController.Summary` on paper to a minimal API. List tests that should still pass.

**Self-grade:** Basic = can sketch route; Solid = preserves `[Authorize]`; Strong = explains OpenAPI and grouping implications.

## Comparison 2: Direct DbContext In Service vs Repository Layer

**The decision this repo made:** infrastructure services use `NetAiOpsDbContext` directly - [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L7-L15).

**The alternative:** introduce `IUserRepository` between services and EF.

```csharp
// Illustrative: not from this repo
public interface IUserRepository
{
    Task<AppUser?> FindActiveByEmailAsync(string email, CancellationToken ct);
}
```

| Dimension | This repo's choice | Alternative |
| --- | --- | --- |
| Complexity / ceremony | Low; EF already is repository/unit-of-work-ish | More interfaces and adapters |
| Testability | Needs test DB or EF provider for service tests | Easier service unit tests with fake repository |
| Performance characteristics | Full access to EF query features | Repository may hide IQueryable and cause poor query shape |
| Coupling & blast radius | Services coupled to EF | Services coupled to repository abstraction |
| Failure modes unique to it | Query logic spreads across services | Leaky generic repositories or underpowered methods |
| Migration cost | Low to add per aggregate later | Premature now |

**What each choice commits you to / who pays:** Direct DbContext makes infrastructure services responsible for query correctness. Repository layer makes maintainers maintain an extra abstraction.

**Verdict for this repo:** Defensible and currently right. There is one simple query in [AuthService.LoginAsync](../../../src/Infrastructure/Auth/AuthService.cs#L14-L15); a repository would not pay for itself yet.

**Switch signal:** introduce repositories or query objects when multiple services duplicate user lookup rules or when tests become slow/hard to isolate.

**Migration drill:** Plan moving user lookup to `IUserReadRepository`.

1. Add interface in Application or Infrastructure boundary and implementation in Infrastructure.
2. Add tests for active/normalized email lookup.
3. Change [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L7-L10) to depend on the repository.
4. Keep old query behavior covered by integration tests.
5. Roll back by restoring direct DbContext dependency if repository abstraction obscures EF constraints.

**Self-grade:** Basic = lists files; Solid = preserves behavior and tests; Strong = explains why generic repositories are often harmful over EF.

## Comparison 3: JWT In localStorage vs httpOnly Cookie

**The decision this repo made:** frontend stores the bearer token in localStorage - [authStore.setSession](../../../frontend/src/app/authStore.ts#L38-L47).

**The alternative:** API sets an httpOnly secure cookie; frontend relies on browser cookie handling.

```csharp
// Illustrative: not from this repo
Response.Cookies.Append("netaiops_session", token, new CookieOptions
{
    HttpOnly = true,
    Secure = true,
    SameSite = SameSiteMode.Strict
});
```

| Dimension | This repo's choice | Alternative |
| --- | --- | --- |
| Complexity / ceremony | Simple SPA bearer token | Requires cookie/CORS/CSRF design |
| Testability | Easy to mock token in client tests | Needs server/browser integration tests |
| Performance characteristics | No server session lookup | Similar if cookie holds JWT |
| Coupling & blast radius | Client owns token lifecycle | Server owns cookie policy |
| Failure modes unique to it | XSS token theft | CSRF if SameSite/anti-forgery wrong |
| Migration cost | Medium; API client and auth endpoint change | Worth considering before production |

**What each choice commits you to / who pays:** localStorage makes frontend pay for token lifecycle and XSS hardening. Cookies make backend pay for CSRF and cookie policy.

**Verdict for this repo:** Acceptable for a lab, questionable for a production internal app.

**Switch signal:** revisit when adding risky actions like incident close, Slack send, or deployment approval.

**Drill:** Write a threat model for one XSS bug on the dashboard.

**Self-grade:** Basic = identifies XSS; Solid = compares CSRF; Strong = designs migration with auth tests.

## Comparison 4: React Query vs Hand-Rolled Fetch State

**The decision this repo made:** TanStack Query owns server-state fetch lifecycle - [main.tsx](../../../frontend/src/main.tsx#L8-L15) and [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L18).

**The alternative:** component-local `useEffect` and `useState`.

```tsx
// Illustrative: not from this repo
useEffect(() => {
  let cancelled = false;
  setLoading(true);
  getDashboardSummary(token).then(data => {
    if (!cancelled) setData(data);
  });
  return () => { cancelled = true; };
}, [token]);
```

| Dimension | This repo's choice | Alternative |
| --- | --- | --- |
| Complexity / ceremony | Library mental model | More code per component |
| Testability | Query client setup needed | Direct component state easier at first |
| Performance characteristics | Caching, retry, stale time | Risk duplicate fetches and stale closures |
| Coupling & blast radius | Components depend on query keys | Components own all loading/error logic |
| Failure modes unique to it | Bad query keys or stale auth cache | Missing cleanup, race conditions |
| Migration cost | Low now; grows as pages multiply | Not worth switching away |

**What each choice commits you to / who pays:** React Query commits maintainers to query-key discipline. Hand-rolled state commits every component author to solve loading/error/race problems repeatedly.

**Verdict for this repo:** Right call, especially with upcoming tickets/incidents/logs pages.

**Switch signal:** do not switch globally; only use local state for data that is not server state.

**Drill:** Define query keys for future ticket list and ticket detail pages.

**Self-grade:** Basic = names loading/error states; Solid = designs stable keys; Strong = handles sign-out cache clearing.

