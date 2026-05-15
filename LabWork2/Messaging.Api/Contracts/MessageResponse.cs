using Messaging.Api.Models;

namespace Messaging.Api.Contracts;

public sealed record MessageResponse(
    Guid MessageId,
    int ConversationId,
    int SenderId,
    int ReceiverId,
    string Content,
    DateTimeOffset Timestamp,
    MessageStatus Status);
