# Type System And Contracts

The backend uses C# records for API contracts. [OperationsDtos](../../src/Application/Operations/OperationsDtos.cs#L5-L115) is the fastest place to learn what each endpoint means. Request records such as `CreateTicketRequest`, `UpdateTicketStatusRequest`, and `CreateIncidentRequest` separate inbound shape from EF entities.

The frontend currently relies on TypeScript inference around the `request<T>` helper in [api.ts](../../frontend/src/app/api.ts#L24-L41). The generic return type gives each page typed data, but runtime response validation is not implemented. A production hardening exercise is to add Zod schemas for key API responses.

Enums and role strings cross the backend/frontend boundary. [RequestContext](../../src/Application/Common/RequestContext.cs#L5-L18) turns claim strings into capability flags, while [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L22) uses role strings to filter navigation. Keep these aligned when adding roles.
