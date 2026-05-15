using Messaging.Api.Contracts;
using Messaging.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> CreateUser(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await userService.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, user);
    }
}
