namespace NetAiOps.Application.Auth;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
}
