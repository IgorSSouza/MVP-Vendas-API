using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Auth.Common;
using Sales.Application.Companies.Commands.SetupInitialCompany;

namespace Sales.Api.Controllers;

[ApiController]
[Route("api/companies")]
public sealed class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompaniesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "OnboardingOnly")]
    [HttpPost("setup-initial")]
    public async Task<ActionResult<AuthResponse>> SetupInitial(
        [FromBody] SetupInitialCompanyCommand command,
        CancellationToken cancellationToken)
    {
        var userIdValue = User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        command.AuthenticatedUserId = userId;

        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
