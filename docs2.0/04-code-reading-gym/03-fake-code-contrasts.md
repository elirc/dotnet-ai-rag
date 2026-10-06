# Fake Code Contrasts

## Illustrative: Not From This Repo

Bad pattern:

```csharp
public Task CloseIncident(Guid id)
{
    SendEmail("incident closed");
    db.Incidents.Find(id)!.Status = Closed;
    return db.SaveChangesAsync();
}
```

Why it is risky: the email can send even if the database save fails. The repo's current approach writes durable side-effect intent through [AddOutbox](../../src/Infrastructure/Operations/OperationsService.cs#L559-L569) and later processes it in [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52).

## Illustrative: Not From This Repo

Bad pattern:

```tsx
{user.role !== 'Viewer' && <button>Close incident</button>}
```

Why it is incomplete: hiding UI is not authorization. NetAiOps combines frontend role filtering in [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73) with backend policies in [Program](../../src/Api/Program.cs#L69-L74) and service checks in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L517-L543).
