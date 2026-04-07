using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public sealed class SaleItem
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public SaleItemType ItemType { get; set; }
    public Guid ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitCostPrice { get; set; }
    public decimal UnitSalePrice { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Profit { get; set; }
}
