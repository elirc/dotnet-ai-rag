using System.Security.Claims;
using FluentAssertions;
using NetAiOps.Application.Common;

namespace NetAiOps.UnitTests;

public sealed class RequestContextTests
{
    [Theory]
    [InlineData("Admin", true, true)]
    [InlineData("Engineer", true, true)]
    [InlineData("SupportAnalyst", false, true)]
    [InlineData("Viewer", false, false)]
    public void FromClaims_MapsRoleCapabilities(string role, bool isAdminOrEngineer, bool canMutateSupportWork)
    {
        var userId = Guid.NewGuid();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role)
        ], "Test"));

        var context = RequestContext.FromClaims(principal);

        context.UserId.Should().Be(userId);
        context.Role.Should().Be(role);
        context.IsAdminOrEngineer.Should().Be(isAdminOrEngineer);
        context.CanMutateSupportWork.Should().Be(canMutateSupportWork);
    }
}
