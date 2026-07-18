namespace NetAiOps.Application.Auth;

public sealed record AuthUserDto(
    Guid Id,
    string Email,
    string DisplayName,
    string Role);
