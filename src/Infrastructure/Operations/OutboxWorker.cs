using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetAiOps.Infrastructure.Persistence;

namespace NetAiOps.Infrastructure.Operations;

public sealed class OutboxWorker(IServiceScopeFactory scopeFactory, ILogger<OutboxWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessagesAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Outbox worker failed while processing pending messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }

    private async Task ProcessPendingMessagesAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NetAiOpsDbContext>();
        var messages = await db.OutboxMessages
            .Where(x => x.Status == "Pending")
            .OrderBy(x => x.CreatedAt)
            .Take(10)
            .ToListAsync(ct);

        foreach (var message in messages)
        {
            logger.LogInformation("Processing outbox message {OutboxMessageId} of type {OutboxMessageType}", message.Id, message.Type);
            message.Status = "Processed";
            message.ProcessedAt = DateTimeOffset.UtcNow;
        }

        if (messages.Count > 0)
        {
            await db.SaveChangesAsync(ct);
        }
    }
}
