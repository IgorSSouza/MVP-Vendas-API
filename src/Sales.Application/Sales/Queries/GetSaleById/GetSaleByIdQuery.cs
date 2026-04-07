using MediatR;
using Sales.Application.Sales.Common;

namespace Sales.Application.Sales.Queries.GetSaleById;

public sealed class GetSaleByIdQuery : IRequest<SaleResponse?>
{
    public Guid Id { get; set; }
}
