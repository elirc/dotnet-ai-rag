using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;

namespace NetAiOps.Api.Controllers;

[ApiController]
[Route("api/approvals")]
[Authorize(Policy = "CanInvestigate")]
public sealed class ApprovalsController(IOperationsService operations) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HumanApprovalDto>>> List(CancellationToken ct)
    {
        return Ok(await operations.ListApprovalsAsync(RequestContext.FromClaims(User), ct));
    }
}
