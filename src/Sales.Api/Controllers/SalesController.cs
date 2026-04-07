using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Sales.Commands.CreateSale;
using Sales.Application.Sales.Common;
using Sales.Application.Sales.Queries.GetAllSales;
using Sales.Application.Sales.Queries.GetSaleById;

namespace Sales.Api.Controllers;

[ApiController]
[Route("api/sales")]
public sealed class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SaleSummaryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var sales = await _mediator.Send(new GetAllSalesQuery(), cancellationToken);
        return Ok(sales);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SaleResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var sale = await _mediator.Send(new GetSaleByIdQuery { Id = id }, cancellationToken);

        return sale is null ? NotFound() : Ok(sale);
    }

    [HttpPost]
    public async Task<ActionResult<SaleResponse>> Create(
        [FromBody] CreateSaleCommand command,
        CancellationToken cancellationToken)
    {
        var sale = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
    }
}
