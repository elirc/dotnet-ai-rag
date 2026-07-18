# Refactor And Design Katas

## Kata 1: Move Startup Migration Out Of API

Resembles [Program seeding](../../../src/Api/Program.cs#L104-L109).

Self-grade: Basic = identifies risk; Solid = proposes dev/prod split; Strong = includes rollback and multi-instance safety.

## Kata 2: Introduce Resource Authorization For Future Tickets

Start from role policies in [Program](../../../src/Api/Program.cs#L46-L51). Design what happens when SupportAnalyst can only see assigned tickets.

## Kata 3: Convert One Controller To Minimal API

Use [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17). Preserve auth and OpenAPI behavior.

## Kata 4: Write An RFC For Cookie Auth Migration

Use comparison card 3 and [authStore](../../../frontend/src/app/authStore.ts#L38-L52). Include CSRF tradeoffs.

