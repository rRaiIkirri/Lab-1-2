using Messaging.Api.Contracts;
using Messaging.Api.Data;
using Messaging.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Api.Services;

public sealed class MessageService(MessagingDbContext dbContext) : IMessageService
{
    public async Task<MessageResponse> SendAsync(
        SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        var content = request.Content?.Trim();
        if (string.IsNullOrWhiteSpace(content))
        {
            throw AppException.BadRequest("Message content must not be empty.");
        }

        var userIds = await dbContext.Users
            .Where(user => user.Id == request.SenderId || user.Id == request.ReceiverId)
            .Select(user => user.Id)
            .ToListAsync(cancellationToken);

        if (!userIds.Contains(request.SenderId))
        {
            throw AppException.NotFound($"Sender with id '{request.SenderId}' was not found.");
        }

        if (!userIds.Contains(request.ReceiverId))
        {
            throw AppException.NotFound($"Receiver with id '{request.ReceiverId}' was not found.");
        }

        var conversation = await GetOrCreateConversationAsync(
            request.SenderId,
            request.ReceiverId,
            cancellationToken);

        var message = new Message
        {
            MessageId = Guid.NewGuid(),
            Conversation = conversation,
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Content = content,
            Timestamp = DateTimeOffset.UtcNow,
            Status = MessageStatus.Sent
        };

        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync(cancellationToken);

        return message.ToResponse();
    }

    public async Task<IReadOnlyList<MessageResponse>> GetConversationHistoryAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .AnyAsync(user => user.Id == userId, cancellationToken);

        if (!userExists)
        {
            throw AppException.NotFound($"User with id '{userId}' was not found.");
        }

        return await dbContext.Messages
            .AsNoTracking()
            .Where(message => message.SenderId == userId || message.ReceiverId == userId)
            .OrderBy(message => message.Timestamp)
            .ThenBy(message => message.MessageId)
            .Select(message => message.ToResponse())
            .ToListAsync(cancellationToken);
    }

    public async Task<MessageResponse> UpdateStatusAsync(
        Guid messageId,
        UpdateMessageStatusRequest request,
        CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages
            .FirstOrDefaultAsync(existingMessage => existingMessage.MessageId == messageId, cancellationToken);

        if (message is null)
        {
            throw AppException.NotFound($"Message with id '{messageId}' was not found.");
        }

        if (message.Status == request.Status)
        {
            return message.ToResponse();
        }

        if (!CanTransition(message.Status, request.Status))
        {
            throw AppException.BadRequest($"Cannot update message status from '{message.Status}' to '{request.Status}'.");
        }

        message.Status = request.Status;
        await dbContext.SaveChangesAsync(cancellationToken);

        return message.ToResponse();
    }

    private static bool CanTransition(MessageStatus currentStatus, MessageStatus requestedStatus) =>
        (int)requestedStatus == (int)currentStatus + 1;

    private async Task<Conversation> GetOrCreateConversationAsync(
        int senderId,
        int receiverId,
        CancellationToken cancellationToken)
    {
        var userAId = Math.Min(senderId, receiverId);
        var userBId = Math.Max(senderId, receiverId);

        var conversation = await dbContext.Conversations
            .FirstOrDefaultAsync(existingConversation =>
                existingConversation.UserAId == userAId &&
                existingConversation.UserBId == userBId,
                cancellationToken);

        if (conversation is not null)
        {
            return conversation;
        }

        conversation = new Conversation
        {
            UserAId = userAId,
            UserBId = userBId
        };

        dbContext.Conversations.Add(conversation);
        return conversation;
    }
}
