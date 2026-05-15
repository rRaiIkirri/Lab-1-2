using System.ComponentModel.DataAnnotations;

namespace Messaging.Api.Contracts;

public sealed record SendMessageRequest(
    [Range(1, int.MaxValue)]
    int SenderId,

    [Range(1, int.MaxValue)]
    int ReceiverId,

    [Required]
    [StringLength(2_000, MinimumLength = 1)]
    string Content);
