# Testing Strategy

## Current Coverage

Only password hashing has direct unit coverage: [BCryptPasswordHasherTests](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L18). That is useful but narrow.

## What Belongs Where

| Test Type | Belongs Here | Example |
| --- | --- | --- |
| Unit | Pure service/util behavior | [BCryptPasswordHasher](../../../src/Infrastructure/Auth/BCryptPasswordHasher.cs#L5-L10). |
| Integration | API + auth + EF + test database | [AuthController](../../../src/Api/Controllers/AuthController.cs#L11-L17), [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17). |
| UI component | React states and interactions | [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L45-L59), [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L13-L19). |
| Contract | DTO shape stability | [DashboardSummaryDto](../../../src/Application/Dashboard/DashboardSummaryDto.cs#L3-L18). |
| Migration | Schema expectations | [InitialIdentity](../../../src/Infrastructure/Persistence/Migrations/20260614190221_InitialIdentity.cs#L14-L36). |

## Do Not Test

Do not assert exact BCrypt hash text; [BCryptPasswordHasherTests](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L13-L16) correctly verifies behavior instead.

