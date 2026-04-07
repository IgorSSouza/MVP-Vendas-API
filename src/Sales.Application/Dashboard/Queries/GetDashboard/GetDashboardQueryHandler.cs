using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Dashboard.Common;

namespace Sales.Application.Dashboard.Queries.GetDashboard;

public sealed class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardResponse>
{
    private readonly IAppDbContext _dbContext;

    public GetDashboardQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DashboardResponse> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var todayStart = now.Date;
        var todayEnd = todayStart.AddDays(1);
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        var salesQuery = _dbContext.Sales.AsNoTracking();
        var productsQuery = _dbContext.Products.AsNoTracking();
        var salesSnapshot = await salesQuery
            .Select(sale => new
            {
                sale.Id,
                sale.CreatedAt,
                sale.PaymentMethod,
                sale.Total,
                sale.Profit,
                ItemCount = sale.Items.Count
            })
            .ToListAsync(cancellationToken);

        var totalSalesCount = salesSnapshot.Count;
        var todaySalesCount = salesSnapshot.Count(sale => sale.CreatedAt >= todayStart && sale.CreatedAt < todayEnd);
        var monthSalesCount = salesSnapshot.Count(sale => sale.CreatedAt >= monthStart && sale.CreatedAt < monthEnd);

        var grossRevenue = salesSnapshot.Sum(sale => sale.Total);
        var estimatedProfit = salesSnapshot.Sum(sale => sale.Profit);

        var salesByPaymentMethod = salesSnapshot
            .GroupBy(sale => sale.PaymentMethod)
            .Select(group => new PaymentMethodSalesResponse(
                group.Key,
                group.Count(),
                group.Sum(sale => sale.Total)))
            .OrderByDescending(item => item.Total)
            .ToList();

        var lowStockProducts = (await productsQuery
            .Where(product => product.IsActive && product.StockQuantity <= 3)
            .Select(product => new LowStockProductResponse(
                product.Id,
                product.Name,
                product.StockQuantity))
            .ToListAsync(cancellationToken))
            .OrderBy(product => product.StockQuantity)
            .ToList();

        var recentSales = salesSnapshot
            .OrderByDescending(sale => sale.CreatedAt)
            .Take(5)
            .Select(sale => new RecentSaleResponse(
                sale.Id,
                sale.CreatedAt,
                sale.PaymentMethod,
                sale.Total,
                sale.Profit,
                sale.ItemCount))
            .ToList();

        return new DashboardResponse(
            totalSalesCount,
            todaySalesCount,
            monthSalesCount,
            grossRevenue,
            estimatedProfit,
            salesByPaymentMethod,
            lowStockProducts,
            recentSales);
    }
}
