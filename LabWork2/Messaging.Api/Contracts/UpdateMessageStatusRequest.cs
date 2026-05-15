using System.ComponentModel.DataAnnotations;
using Messaging.Api.Models;

namespace Messaging.Api.Contracts;

public sealed record UpdateMessageStatusRequest(
    [EnumDataType(typeof(MessageStatus))]
    MessageStatus Status);
