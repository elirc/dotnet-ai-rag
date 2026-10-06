# Review Katas

## Kata 1: Ticket Status Change

Review [OperationsService.UpdateTicketStatusAsync](../../src/Infrastructure/Operations/OperationsService.cs#L122-L147). Questions:

- What happens if a Viewer calls this endpoint?
- Where is status transition validation?
- Does the outbox payload include enough information for downstream systems?
- What test is missing?

## Kata 2: Log Search

Review [OperationsService.SearchLogsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L439-L466). Questions:

- Are page bounds enforced?
- Which fields can be filtered?
- What index choices support this?
- What happens if query length is huge?

## Kata 3: Frontend Settings Page

Review [AdminSettingsPage](../../frontend/src/pages/AdminSettingsPage.tsx#L6-L27). Questions:

- Is it read-only?
- Is route access role-filtered?
- What server-side policy should protect the endpoint?
- What loading and error states exist?
