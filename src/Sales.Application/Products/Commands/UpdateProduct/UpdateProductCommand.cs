using MediatR;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommand : IRequest<ProductResponse>
{
    public Guid Id { get; set; }

    public string? Barcode { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public decimal CostPrice { get; set; }

    public decimal SalePrice { get; set; }

    public decimal StockQuantity { get; set; }
}
