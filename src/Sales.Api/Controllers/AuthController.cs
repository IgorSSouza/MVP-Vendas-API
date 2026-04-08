using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Auth.Commands.AuthenticateWithGoogle;
using Sales.Application.Auth.Common;

namespace Sales.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("google")]
    public async Task<ActionResult<AuthResponse>> AuthenticateWithGoogle(
        [FromBody] AuthenticateWithGoogleCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
