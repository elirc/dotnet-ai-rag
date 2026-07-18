namespace NetAiOps.Domain.Incidents;

public enum IncidentSeverity
{
    SEV1 = 1,
    SEV2 = 2,
    SEV3 = 3,
    SEV4 = 4
}

public enum IncidentStatus
{
    Detected = 1,
    Investigating = 2,
    Mitigating = 3,
    Resolved = 4,
    PostmortemPending = 5,
    Closed = 6
}
