using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Dashboard.Common;
using Sales.Application.Dashboard.Queries.GetDashboard;

namespace Sales.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardResponse>> Get(CancellationToken cancellationToken)
    {
        var dashboard = await _mediator.Send(new GetDashboardQuery(), cancellationToken);
        return Ok(dashboard);
    }
}
