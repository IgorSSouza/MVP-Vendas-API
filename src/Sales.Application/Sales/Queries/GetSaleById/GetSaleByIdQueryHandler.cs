using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Sales.Common;

namespace Sales.Application.Sales.Queries.GetSaleById;

public sealed class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleResponse?>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public GetSaleByIdQueryHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<SaleResponse?> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        var sale = await _dbContext.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == companyId, cancellationToken);

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
