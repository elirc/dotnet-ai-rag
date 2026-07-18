using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;

namespace NetAiOps.Api.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
public sealed class TicketsController(IOperationsService operations) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<TicketListItemDto>>> List([FromQuery] string? status, [FromQuery] string? priority, [FromQuery] int page = 1, [FromQuery] int pageSize = 25, CancellationToken ct = default)
    {
        return Ok(await operations.ListTicketsAsync(RequestContext.FromClaims(User), status, priority, page, pageSize, ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TicketDetailDto>> Get(Guid id, CancellationToken ct)
    {
        var result = await operations.GetTicketAsync(RequestContext.FromClaims(User), id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "CanUseSupportTools")]
    public async Task<ActionResult<TicketDetailDto>> Create(CreateTicketRequest request, CancellationToken ct)
    {
        var result = await operations.CreateTicketAsync(RequestContext.FromClaims(User), request, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/status")]
    [Authorize(Policy = "CanUseSupportTools")]
    public async Task<ActionResult<TicketDetailDto>> UpdateStatus(Guid id, UpdateTicketStatusRequest request, CancellationToken ct)
    {
        var result = await operations.UpdateTicketStatusAsync(RequestContext.FromClaims(User), id, request, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:guid}/assignment")]
    [Authorize(Policy = "CanUseSupportTools")]
    public async Task<ActionResult<TicketDetailDto>> Assign(Guid id, AssignTicketRequest request, CancellationToken ct)
    {
        var result = await operations.AssignTicketAsync(RequestContext.FromClaims(User), id, request, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:guid}/comments")]
    [Authorize(Policy = "CanUseSupportTools")]
    public async Task<ActionResult<TicketDetailDto>> AddComment(Guid id, AddTicketCommentRequest request, CancellationToken ct)
    {
        var result = await operations.AddTicketCommentAsync(RequestContext.FromClaims(User), id, request, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
