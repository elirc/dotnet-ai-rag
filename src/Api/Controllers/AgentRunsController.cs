using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;

namespace NetAiOps.Api.Controllers;

[ApiController]
[Route("api/agents/runs")]
[Authorize]
public sealed class AgentRunsController(IOperationsService operations) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AgentRunDto>>> List(CancellationToken ct)
    {
        return Ok(await operations.ListAgentRunsAsync(RequestContext.FromClaims(User), ct));
    }
}
