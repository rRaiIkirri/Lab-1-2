using Messaging.Api.Contracts;

namespace Messaging.Api.Services;

public interface IMessageService
{
    Task<MessageResponse> SendAsync(SendMessageRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyList<MessageResponse>> GetConversationHistoryAsync(
        int userId,
        CancellationToken cancellationToken);

    Task<MessageResponse> UpdateStatusAsync(
        Guid messageId,
        UpdateMessageStatusRequest request,
        CancellationToken cancellationToken);
}
