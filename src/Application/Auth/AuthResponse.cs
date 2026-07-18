namespace NetAiOps.Application.Auth;

public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    AuthUserDto User);
