using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Services.Commands.CreateService;
using Sales.Application.Services.Commands.ToggleServiceStatus;
using Sales.Application.Services.Commands.UpdateService;
using Sales.Application.Services.Common;
using Sales.Application.Services.Queries.GetAllServices;
using Sales.Application.Services.Queries.GetServiceById;

namespace Sales.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/services")]
public sealed class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ServiceResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var services = await _mediator.Send(new GetAllServicesQuery(), cancellationToken);
        return Ok(services);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var service = await _mediator.Send(new GetServiceByIdQuery { Id = id }, cancellationToken);

        return service is null ? NotFound() : Ok(service);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse>> Create(
        [FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken)
    {
        var service = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> Update(
        Guid id,
        [FromBody] UpdateServiceCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var service = await _mediator.Send(command, cancellationToken);
        return Ok(service);
    }

    [HttpPatch("{id:guid}/toggle-status")]
    public async Task<ActionResult<ServiceResponse>> ToggleStatus(
        Guid id,
        CancellationToken cancellationToken)
    {
        var service = await _mediator.Send(new ToggleServiceStatusCommand { Id = id }, cancellationToken);
        return Ok(service);
    }
}
