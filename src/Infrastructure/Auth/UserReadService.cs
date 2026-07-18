using Microsoft.EntityFrameworkCore;
using NetAiOps.Application.Users;
using NetAiOps.Infrastructure.Persistence;

namespace NetAiOps.Infrastructure.Auth;

public sealed class UserReadService(NetAiOpsDbContext db) : IUserReadService
{
    public async Task<IReadOnlyList<UserDto>> ListUsersAsync(CancellationToken ct)
    {
        return await db.Users
            .OrderBy(x => x.Role)
            .ThenBy(x => x.DisplayName)
            .Select(x => new UserDto(
                x.Id,
                x.Email,
                x.DisplayName,
                x.Role.ToString(),
                x.IsActive,
                x.CreatedAt))
            .ToListAsync(ct);
    }
}
