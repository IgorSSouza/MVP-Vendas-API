using Sales.Domain.Enums;

namespace Sales.Application.Sales.Commands.CreateSale;

public sealed class CreateSaleItemRequest
{
    public SaleItemType ItemType { get; set; }

    public Guid ItemId { get; set; }

    public decimal Quantity { get; set; }
}
