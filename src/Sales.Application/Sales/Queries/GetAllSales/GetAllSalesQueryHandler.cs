using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Sales.Common;

namespace Sales.Application.Sales.Queries.GetAllSales;

public sealed class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, IReadOnlyList<SaleSummaryResponse>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllSalesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<SaleSummaryResponse>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Sales
            .OrderByDescending(sale => sale.CreatedAt)
            .Select(sale => new SaleSummaryResponse(
                sale.Id,
                sale.CreatedAt,
                sale.PaymentMethod,
                sale.Subtotal,
                sale.Discount,
                sale.Total,
                sale.Profit,
                sale.Items.Count))
            .ToListAsync(cancellationToken);
    }
}
