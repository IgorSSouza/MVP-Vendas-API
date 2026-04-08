using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Sales.Common;

namespace Sales.Application.Sales.Queries.GetAllSales;

public sealed class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, IReadOnlyList<SaleSummaryResponse>>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public GetAllSalesQueryHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<IReadOnlyList<SaleSummaryResponse>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        return await _dbContext.Sales
            .Where(sale => sale.CompanyId == companyId)
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
