# Learning Rubrics

## Junior Observable Behaviors

- Can run verified build/test commands.
- Can trace login from [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L16-L69) to [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24).
- Can explain why [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18) is not server security.
- Can make a small UI copy change and run `npm run build`.

## Mid-Level Observable Behaviors

- Designs a cross-layer feature with entity, migration, DTO, service, controller, UI, and tests.
- Adds integration tests around [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17) or [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17).
- Explains tradeoffs in [Comparative Implementations](../03-architecture-and-patterns/07-comparative-implementations.md).
- Identifies risks before implementation.

## Senior Observable Behaviors

- Challenges startup migration policy using [Program](../../../src/Api/Program.cs#L104-L109) and [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L40).
- Produces rollout and rollback plans.
- Distinguishes confirmed findings from hypotheses.
- Connects local choices to future cost, ownership, and blast radius.

## Self-Assessment

| Prompt | Basic | Solid | Strong |
| --- | --- | --- | --- |
| Trace login | Names files | Explains data transformations | Adds failure modes and tests |
| Review auth PR | Spots missing `[Authorize]` | Requires role/resource check | Discusses threat model and auditability |
| Design tickets | Adds CRUD | Adds validation/tests | Adds indexes, authz, migration safety, observability |

