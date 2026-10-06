# Senior Build Projects

## Project 1: Split OperationsService

[OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L14-L569) is a productive but broad service. Split it into focused services without changing API behavior.

Deliverables: proposed boundaries, migration plan, compatibility tests, and PR notes explaining why the split reduces risk.

## Project 2: API Integration Test Harness

Build tests that boot the API, authenticate seeded roles, and verify policies around tickets, incidents, logs, and admin settings. Anchor policies in [Program](../../src/Api/Program.cs#L69-L74) and service checks in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L517-L543).

## Project 3: Agent Evaluation Loop

Seeded eval runs exist in [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L185-L214) and are displayed by [EvalsPage](../../frontend/src/pages/EvalsPage.tsx#L6-L29). Turn this into a real regression harness for incident investigation output.
