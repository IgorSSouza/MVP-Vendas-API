using Microsoft.AspNetCore.Mvc;
using Sales.Application.Abstractions.Health;

namespace Sales.Api.Controllers;

[ApiController]
[Route("status")]
public sealed class StatusController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices] IDatabaseHealthChecker databaseHealthChecker,
        CancellationToken cancellationToken)
    {
        var databaseConnected = await databaseHealthChecker.CanConnectAsync(cancellationToken);

        return Ok(new
        {
            status = "ok",
            service = "sales-api",
            database = databaseConnected ? "connected" : "unreachable",
            localNow = DateTime.Now
        });
    }
}
