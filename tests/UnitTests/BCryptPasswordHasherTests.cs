using FluentAssertions;
using NetAiOps.Infrastructure.Auth;

namespace NetAiOps.UnitTests;

public sealed class BCryptPasswordHasherTests
{
    [Fact]
    public void Verify_ReturnsTrue_ForMatchingPassword()
    {
        var hasher = new BCryptPasswordHasher();

        var hash = hasher.Hash("Password123!");

        hasher.Verify("Password123!", hash).Should().BeTrue();
        hasher.Verify("wrong-password", hash).Should().BeFalse();
    }
}
