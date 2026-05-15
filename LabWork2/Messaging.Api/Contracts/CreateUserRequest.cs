using System.ComponentModel.DataAnnotations;

namespace Messaging.Api.Contracts;

public sealed record CreateUserRequest(
    [Required]
    [StringLength(50, MinimumLength = 2)]
    string Username);
