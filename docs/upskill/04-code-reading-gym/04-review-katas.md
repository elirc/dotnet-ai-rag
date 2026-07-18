# Review Katas

## Kata 1: Add Admin Settings Endpoint

**Author intent:** expose settings to the UI.

**Fake diff summary:** adds `SettingsController` without `[Authorize]`.

**Resembles:** [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17).

**Expected findings:** Blocking: missing Admin policy. Important: no DTO validation. Optional: naming.

**Good review comment example:**

> Blocking: this endpoint exposes admin settings but lacks the `AdminOnly` policy used by `UsersController`. Please add `[Authorize(Policy = "AdminOnly")]` and a permission test.

## Kata 2: Store Full Auth Response In Component State Only

**Author intent:** avoid global state.

**Resembles:** [authStore](../../../frontend/src/app/authStore.ts#L38-L52).

**Expected findings:** Important: loses session on refresh; can break protected routes.

## Kata 3: Add Dashboard API Fetch Without Error UI

**Author intent:** display metrics quickly.

**Resembles:** [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L13-L19).

**Expected findings:** Important: missing loading/error states.

## Kata 4: Seed User Every Startup Without Existing Check

**Author intent:** ensure seed users exist.

**Resembles:** [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L22-L40).

**Expected findings:** Blocking: duplicate rows/unique index failure. Good code is idempotent.

