namespace Messaging.Api.Models;

public sealed class Message
{
    public Guid MessageId { get; set; }

    public int ConversationId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset Timestamp { get; set; }

    public MessageStatus Status { get; set; }

    public User? Sender { get; set; }

    public User? Receiver { get; set; }

    public Conversation? Conversation { get; set; }
}
