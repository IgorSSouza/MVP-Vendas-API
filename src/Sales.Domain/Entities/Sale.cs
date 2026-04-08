using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public sealed class Sale
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Discount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public decimal Profit { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}
