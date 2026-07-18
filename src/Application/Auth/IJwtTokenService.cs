namespace NetAiOps.Application.Auth;

public interface IJwtTokenService
{
    AuthResponse CreateToken(AuthUserDto user);
}
