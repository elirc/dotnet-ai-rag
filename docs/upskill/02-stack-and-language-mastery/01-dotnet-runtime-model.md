# .NET Runtime Model

## CLR, JIT, And Allocation

The backend is .NET 8 according to [NetAiOps.Api.csproj](../../../src/Api/NetAiOps.Api.csproj#L3-L7). At runtime, ASP.NET Core builds a service provider, JIT-compiles methods on demand, and allocates request objects, DTOs, EF objects, and JWT objects as requests flow through the app.

Watch allocations around token creation: [JwtTokenService.CreateToken](../../../src/Infrastructure/Auth/JwtTokenService.cs#L14-L35) allocates claims, signing credentials, token, and response. That is fine for this lab; in high-volume auth systems, measure before optimizing.

## Async/Await And CancellationToken

The implemented async flow is conventional: controller methods accept `CancellationToken`, then services pass it to EF. Example: [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L13-L17) calls [AuthService.LoginAsync](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24), which passes `ct` to `SingleOrDefaultAsync`.

Failure mode: forgetting to pass cancellation tokens makes aborted HTTP requests continue doing database work.

## Task vs ValueTask

This repo uses `Task`, including [DashboardService.GetSummaryAsync](../../../src/Infrastructure/Persistence/DashboardService.cs#L7-L35). That method returns `Task.FromResult` because it is currently static. Do not switch to `ValueTask` casually; it adds complexity and rarely matters before profiling.

## Drill

Find every `CancellationToken` in the login path. Explain where cancellation stops and where it would not help.

