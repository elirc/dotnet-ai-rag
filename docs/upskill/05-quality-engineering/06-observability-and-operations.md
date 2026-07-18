# Observability And Operations

## Current Signals

ASP.NET logging levels are configured in [appsettings](../../../src/Api/appsettings.json#L17-L22). Swagger is enabled in development in [Program](../../../src/Api/Program.cs#L90-L94). There is no custom exception middleware, correlation ID middleware, health check, or structured audit trail yet.

## How Would I Know This Broke?

| Flow | Break Signal | Missing Signal To Add |
| --- | --- | --- |
| Login | 401 spike, user reports | auth failure counters without logging passwords |
| Dashboard | frontend error state in [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L17-L19) | API request correlation ID |
| Startup seeding | app fails before listening | migration timing and DB readiness logs |
| Admin users | 403/401 responses | role decision logs at debug level |

## Drill

Design a correlation ID middleware placement. It should run before [UseAuthentication](../../../src/Api/Program.cs#L99-L100) so auth failures also get correlated.

