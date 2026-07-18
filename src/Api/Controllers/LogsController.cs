using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;

namespace NetAiOps.Api.Controllers;

[ApiController]
[Route("api/logs")]
[Authorize]
public sealed class LogsController(IOperationsService operations) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<LogEntryDto>>> Search([FromQuery] string? serviceName, [FromQuery] string? level, [FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        return Ok(await operations.SearchLogsAsync(RequestContext.FromClaims(User), serviceName, level, query, page, pageSize, ct));
    }
}
