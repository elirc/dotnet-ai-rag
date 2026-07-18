using System.Security.Claims;

namespace NetAiOps.Application.Common;

public sealed record RequestContext(Guid UserId, string Role)
{
    public static RequestContext FromClaims(ClaimsPrincipal user)
    {
        var rawId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("Authenticated user is missing a name identifier claim.");
        var role = user.FindFirst(ClaimTypes.Role)?.Value ?? "Viewer";

        return new RequestContext(Guid.Parse(rawId), role);
    }

    public bool IsAdminOrEngineer => Role is "Admin" or "Engineer";
    public bool CanMutateSupportWork => Role is "Admin" or "Engineer" or "SupportAnalyst";
}
