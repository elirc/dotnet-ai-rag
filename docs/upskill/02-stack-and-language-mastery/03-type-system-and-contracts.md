# Type System And Contracts

## C# Nullable And Records

Nullable reference types are enabled in [NetAiOps.Api.csproj](../../../src/Api/NetAiOps.Api.csproj#L3-L7). DTOs use records, for example [DashboardSummaryDto](../../../src/Application/Dashboard/DashboardSummaryDto.cs#L3-L18). Records are good for immutable-ish transport shapes and equality semantics.

## Required Properties

[AppUser](../../../src/Domain/Users/AppUser.cs#L3-L13) uses `required` for string properties. That helps object initialization but does not validate email format or password hash quality by itself.

## TypeScript Types

The frontend API contract is modeled in [types.ts](../../../frontend/src/app/types.ts) (opened by file inventory but not line-numbered in this pass; treat detailed claims as inferred). Concrete usage appears in [api.ts](../../../frontend/src/app/api.ts#L1-L37), where generic `request<T>` returns typed promises.

## Runtime vs Compile-Time Validation

TypeScript and C# types catch programmer mistakes. Runtime validation catches untrusted input. [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L9-L12) validates client-side with Zod; [LoginRequest](../../../src/Application/Auth/LoginRequest.cs#L5-L7) validates server-side with annotations.

## Drill

Add a `DisplayName` minimum length rule on paper. Which layer gets UX validation? Which layer protects the API?

