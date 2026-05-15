using Messaging.Api.Contracts;
using Messaging.Api.Models;

namespace Messaging.Api.Services;

internal static class MappingExtensions
{
    public static UserResponse ToResponse(this User user) =>
        new(user.Id, user.Username);

    public static MessageResponse ToResponse(this Message message) =>
        new(
            message.MessageId,
            message.ConversationId,
            message.SenderId,
            message.ReceiverId,
            message.Content,
            message.Timestamp,
            message.Status);
}
