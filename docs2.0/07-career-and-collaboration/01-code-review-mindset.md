# Code Review Mindset

Review for behavior first, then safety, then maintainability. For example, when reviewing [OperationsService.UpdateTicketStatusAsync](../../src/Infrastructure/Operations/OperationsService.cs#L120-L145), ask:

- Can the wrong role call this?
- Are invalid transitions blocked?
- Are audit and outbox side effects written with the state change?
- Is there a test that proves the risky path?

When reviewing frontend pages like [IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L6-L43), ask whether the UI state matches backend truth and whether mutation success invalidates the right cached data.

Good review comments are specific: cite the line, explain the user or production risk, and suggest a small path forward.
