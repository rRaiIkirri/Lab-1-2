using Messaging.Api.Contracts;
using Messaging.Api.Data;
using Messaging.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Api.Services;

public sealed class UserService(MessagingDbContext dbContext) : IUserService
{
    public async Task<UserResponse> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var username = request.Username?.Trim();
        if (string.IsNullOrWhiteSpace(username))
        {
            throw AppException.BadRequest("Username is required.");
        }

        var usernameExists = await dbContext.Users
            .AnyAsync(user => user.Username == username, cancellationToken);

        if (usernameExists)
        {
            throw AppException.Conflict($"User '{username}' already exists.");
        }

        var user = new User
        {
            Username = username
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return user.ToResponse();
    }
}
