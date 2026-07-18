namespace NetAiOps.Domain.Tickets;

public enum TicketStatus
{
    New = 1,
    Triaged = 2,
    InProgress = 3,
    WaitingOnCustomer = 4,
    Escalated = 5,
    Resolved = 6,
    Closed = 7
}

public enum TicketPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum TicketCategory
{
    LoginIssue = 1,
    PaymentIssue = 2,
    Performance = 3,
    Deployment = 4,
    Database = 5,
    Network = 6,
    Security = 7,
    Unknown = 8
}
