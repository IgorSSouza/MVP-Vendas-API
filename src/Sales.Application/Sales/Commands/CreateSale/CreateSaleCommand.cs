using MediatR;
using Sales.Application.Sales.Common;
using Sales.Domain.Enums;

namespace Sales.Application.Sales.Commands.CreateSale;

public sealed class CreateSaleCommand : IRequest<SaleResponse>
{
    public PaymentMethod PaymentMethod { get; set; }

    public decimal Discount { get; set; }

    public List<CreateSaleItemRequest> Items { get; set; } = [];
}
