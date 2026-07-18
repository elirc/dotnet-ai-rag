# Boundaries And Layers

The current boundary is pragmatic: controllers are thin, application contracts are separate, EF Core sits in infrastructure, and business behavior is implemented in infrastructure services. This is visible from [TicketsController](../../src/Api/Controllers/TicketsController.cs#L8-L55) delegating to [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L12-L493).

The upside is speed and readability for a training repo. The downside is that [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L12-L493) is becoming a large multi-domain service. A mid-size company would likely split this into ticket, incident, log, knowledge, agent, and admin services once ownership or change rate diverges.

Rule of thumb: keep the current shape until a change forces you to understand unrelated workflows. That is the signal to split boundaries.
