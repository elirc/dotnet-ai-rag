using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;

namespace NetAiOps.Api.Controllers;

[ApiController]
[Route("api/evals")]
[Authorize(Policy = "AdminOnly")]
public sealed class EvalsController(IOperationsService operations) : ControllerBase
{
    [HttpGet("runs")]
    public async Task<ActionResult<IReadOnlyList<EvalRunDto>>> ListRuns(CancellationToken ct)
    {
        return Ok(await operations.ListEvalRunsAsync(RequestContext.FromClaims(User), ct));
    }
}
