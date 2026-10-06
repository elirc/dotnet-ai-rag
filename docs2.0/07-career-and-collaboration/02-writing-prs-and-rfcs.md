# Writing PRs And RFCs

PR template for this repo:

```markdown
## What changed

## Why

## How tested

## Risks / rollback

## Screenshots or API examples
```

For risky changes, write a short RFC before code. Example topics: replacing startup migrations in [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L16-L18), splitting [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L14-L569), or changing browser auth behavior in [api.ts](../../frontend/src/app/api.ts#L24-L41).

Use [CONTRIBUTING](../../CONTRIBUTING.md#L13-L20) as the local expectation list.
