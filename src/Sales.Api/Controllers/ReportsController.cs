using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Reports.Common;
using Sales.Application.Reports.Queries.GetRevenueReport;

namespace Sales.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/reports")]
public sealed class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("revenue")]
    public async Task<ActionResult<RevenueReportResponse>> GetRevenue(
        [FromQuery] GetRevenueReportQuery query,
        CancellationToken cancellationToken)
    {
        var report = await _mediator.Send(query, cancellationToken);
        return Ok(report);
    }
}
