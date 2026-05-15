using Messaging.Api.Contracts;
using Messaging.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Api.Controllers;

[ApiController]
[Route("api/conversations")]
public sealed class ConversationsController(IMessageService messageService) : ControllerBase
{
    [HttpGet("{userId:int}")]
    [ProducesResponseType<IReadOnlyList<MessageResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<MessageResponse>>> GetConversationHistory(
        int userId,
        CancellationToken cancellationToken)
    {
        var messages = await messageService.GetConversationHistoryAsync(userId, cancellationToken);
        return Ok(messages);
    }
}
