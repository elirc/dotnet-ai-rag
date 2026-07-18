using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;

namespace NetAiOps.Api.Controllers;

[ApiController]
[Route("api/admin/settings")]
[Authorize(Policy = "AdminOnly")]
public sealed class AdminSettingsController(IOperationsService operations) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminSettingDto>>> List(CancellationToken ct)
    {
        return Ok(await operations.ListAdminSettingsAsync(RequestContext.FromClaims(User), ct));
    }
}
