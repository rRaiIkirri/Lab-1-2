using Messaging.Api.Contracts;
using Messaging.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Api.Controllers;

[ApiController]
[Route("api/messages")]
public sealed class MessagesController(IMessageService messageService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> SendMessage(
        SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        var message = await messageService.SendAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, message);
    }

    [HttpPatch("{messageId:guid}/status")]
    [ProducesResponseType<MessageResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> UpdateStatus(
        Guid messageId,
        UpdateMessageStatusRequest request,
        CancellationToken cancellationToken)
    {
        var message = await messageService.UpdateStatusAsync(messageId, request, cancellationToken);
        return Ok(message);
    }
}
