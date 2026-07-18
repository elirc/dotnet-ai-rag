# Validation Auth And Permissions

JWT validation is configured in [Program](../../src/Api/Program.cs#L41-L67), and authorization policies are configured in [Program](../../src/Api/Program.cs#L69-L74). That gives route-level protection, but service-level checks still matter because business permissions can depend on resource ownership.

[RequestContext](../../src/Application/Common/RequestContext.cs#L5-L18) converts claims into capability flags. [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L441-L467) uses those flags for read and mutation checks. [RequestContextTests](../../tests/UnitTests/RequestContextTests.cs#L9-L29) tests role-to-capability mapping.

Frontend role filtering in [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73) improves UX, but it is not security by itself. Server-side policy and service checks are the source of truth.

Production hardening exercises: add FluentValidation or endpoint-level validation for request bodies, add negative authorization tests for each mutation, and return consistent 403/404 behavior for resource-scoped reads.
