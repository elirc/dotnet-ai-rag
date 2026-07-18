namespace NetAiOps.Application.Users;

public interface IUserReadService
{
    Task<IReadOnlyList<UserDto>> ListUsersAsync(CancellationToken ct);
}
