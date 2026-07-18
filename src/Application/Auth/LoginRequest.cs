using System.ComponentModel.DataAnnotations;

namespace NetAiOps.Application.Auth;

public sealed record LoginRequest(
    [property: Required, EmailAddress] string Email,
    [property: Required] string Password);
