using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;

namespace NetAiOps.Api.Controllers;

[ApiController]
[Route("api/knowledge-documents")]
[Authorize]
public sealed class KnowledgeDocumentsController(IOperationsService operations) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<KnowledgeDocumentDto>>> List(CancellationToken ct)
    {
        return Ok(await operations.ListKnowledgeDocumentsAsync(RequestContext.FromClaims(User), ct));
    }
}
