using Microsoft.EntityFrameworkCore;
using NetAiOps.Application.Auth;
using NetAiOps.Infrastructure.Persistence;

namespace NetAiOps.Infrastructure.Auth;

public sealed class AuthService(
    NetAiOpsDbContext db,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == normalizedEmail && x.IsActive, ct);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        var dto = new AuthUserDto(user.Id, user.Email, user.DisplayName, user.Role.ToString());
        return jwtTokenService.CreateToken(dto);
    }
}
