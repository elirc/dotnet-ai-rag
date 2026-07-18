# Security Checklist

- JWT settings are validated before use in [Program](../../src/Api/Program.cs#L41-L50).
- JWT bearer validation is configured in [Program](../../src/Api/Program.cs#L52-L67).
- Role policies are configured in [Program](../../src/Api/Program.cs#L69-L74).
- Login is rate-limited by policy in [Program](../../src/Api/Program.cs#L19-L31).
- Correlation and security headers are added in [CorrelationAndSecurityHeadersMiddleware](../../src/Api/Middleware/CorrelationAndSecurityHeadersMiddleware.cs#L3-L23).
- Resource checks happen in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L441-L467).

Production gaps to close:

- Add negative authorization tests for every mutation.
- Add request validation for lengths, enum values, and required fields.
- Move away from localStorage bearer tokens for production browser auth, or document compensating controls.
- Store secrets outside appsettings and compose files for real deployments.
- Add audit visibility for admin setting changes.
