namespace Messaging.Api.Models;

public sealed class Conversation
{
    public int Id { get; set; }

    public string Type { get; set; } = "direct";

    public int UserAId { get; set; }

    public int UserBId { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
