using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Sales.Common;

namespace Sales.Application.Sales.Queries.GetSaleById;

public sealed class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleResponse?>
{
    private readonly IAppDbContext _dbContext;

    public GetSaleByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SaleResponse?> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _dbContext.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (sale is null)
        {
            return null;
        }

        return new SaleResponse(
            sale.Id,
            sale.CreatedAt,
            sale.PaymentMethod,
            sale.Subtotal,
            sale.Discount,
            sale.Total,
            sale.Profit,
            sale.Items
                .OrderBy(item => item.Name)
                .Select(item => new SaleItemResponse(
                    item.Id,
                    item.ItemType,
                    item.ItemId,
                    item.Name,
                    item.Quantity,
                    item.UnitCostPrice,
                    item.UnitSalePrice,
                    item.Subtotal,
                    item.Profit))
                .ToList());
    }
}
