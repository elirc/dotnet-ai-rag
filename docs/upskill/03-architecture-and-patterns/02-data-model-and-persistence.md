# Data Model And Persistence

## Implemented Model

The only implemented persistent model is user identity. [AppUser](../../../src/Domain/Users/AppUser.cs#L3-L13) defines the C# shape. [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L12-L30) maps it to the `users` table. [InitialIdentity](../../../src/Infrastructure/Persistence/Migrations/20260614190221_InitialIdentity.cs#L14-L36) creates the durable database schema.

## Persistence Rules

| Concern | Current Choice | Evidence | Senior Question |
| --- | --- | --- | --- |
| Table naming | Explicit `users` | [ToTable](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L12-L14) | Will all future tables use snake_case consistently? |
| Email uniqueness | Unique index | [HasIndex](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L15-L16) | Is uniqueness case-insensitive? |
| Role storage | Enum converted to string | [Role mapping](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L22-L26) | How are role renames migrated? |
| Startup migration | `MigrateAsync` in seeder | [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L11) | Is this safe with multiple API instances? |

## Safe Schema Change Guidance

1. Add nullable column or side table first.
2. Backfill with idempotent code or migration script.
3. Deploy readers that tolerate old and new shapes.
4. Enforce constraints after data is valid.
5. Add tests around migration assumptions.

## Drill

Design the Phase 2 `tickets` table. Include at least one index and explain which query it supports.

