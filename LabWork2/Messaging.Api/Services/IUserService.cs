using Messaging.Api.Contracts;

namespace Messaging.Api.Services;

public interface IUserService
{
    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken);
}
