using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Products.Commands.CreateProduct;
using Sales.Application.Products.Commands.ToggleProductStatus;
using Sales.Application.Products.Commands.UpdateProduct;
using Sales.Application.Products.Common;
using Sales.Application.Products.Queries.GetAllProducts;
using Sales.Application.Products.Queries.GetProductById;

namespace Sales.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new GetProductByIdQuery { Id = id }, cancellationToken);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Update(
        Guid id,
        [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var product = await _mediator.Send(command, cancellationToken);
        return Ok(product);
    }

    [HttpPatch("{id:guid}/toggle-status")]
    public async Task<ActionResult<ProductResponse>> ToggleStatus(
        Guid id,
        CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new ToggleProductStatusCommand { Id = id }, cancellationToken);
        return Ok(product);
    }
}
