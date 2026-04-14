using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Reports.Common;

namespace Sales.Application.Reports.Queries.GetRevenueReport;

public sealed class GetRevenueReportQueryHandler : IRequestHandler<GetRevenueReportQuery, RevenueReportResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public GetRevenueReportQueryHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<RevenueReportResponse> Handle(GetRevenueReportQuery request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        var startDate = request.StartDate!.Value.Date;
        var endDate = request.EndDate!.Value.Date;
        var endExclusive = endDate.AddDays(1);

        var salesSnapshot = await _dbContext.Sales
            .AsNoTracking()
            .Where(sale => sale.CompanyId == companyId)
            .Where(sale => sale.CreatedAt >= startDate && sale.CreatedAt < endExclusive)
            .Select(sale => new
            {
                sale.PaymentMethod,
                sale.Subtotal,
                sale.Total,
                sale.Profit
            })
            .ToListAsync(cancellationToken);

        var paymentMethodBreakdown = salesSnapshot
            .GroupBy(sale => sale.PaymentMethod)
            .Select(group => new RevenuePaymentMethodBreakdownResponse(
                group.Key,
                group.Count(),
                group.Sum(sale => sale.Subtotal),
                group.Sum(sale => sale.Total),
                group.Sum(sale => sale.Profit)))
            .OrderByDescending(item => item.TotalAmount)
            .ToList();

        return new RevenueReportResponse(
            startDate,
            endDate,
            salesSnapshot.Count,
            salesSnapshot.Sum(sale => sale.Subtotal),
            salesSnapshot.Sum(sale => sale.Total),
            salesSnapshot.Sum(sale => sale.Profit),
            paymentMethodBreakdown);
    }
}
