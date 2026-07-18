using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;

namespace NetAiOps.Api.Controllers;

[ApiController]
[Route("api/incidents")]
[Authorize]
public sealed class IncidentsController(IOperationsService operations) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<IncidentListItemDto>>> List([FromQuery] string? status, [FromQuery] string? severity, [FromQuery] int page = 1, [FromQuery] int pageSize = 25, CancellationToken ct = default)
    {
        return Ok(await operations.ListIncidentsAsync(RequestContext.FromClaims(User), status, severity, page, pageSize, ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IncidentDetailDto>> Get(Guid id, CancellationToken ct)
    {
        var result = await operations.GetIncidentAsync(RequestContext.FromClaims(User), id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "CanInvestigate")]
    public async Task<ActionResult<IncidentDetailDto>> Create(CreateIncidentRequest request, CancellationToken ct)
    {
        var result = await operations.CreateIncidentAsync(RequestContext.FromClaims(User), request, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/timeline")]
    [Authorize(Policy = "CanInvestigate")]
    public async Task<ActionResult<IncidentDetailDto>> AddTimeline(Guid id, AddIncidentTimelineRequest request, CancellationToken ct)
    {
        var result = await operations.AddIncidentTimelineAsync(RequestContext.FromClaims(User), id, request, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:guid}/run-investigation")]
    [Authorize(Policy = "CanInvestigate")]
    public async Task<ActionResult<AgentRunDto>> RunInvestigation(Guid id, CancellationToken ct)
    {
        var result = await operations.RunIncidentInvestigationAsync(RequestContext.FromClaims(User), id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
