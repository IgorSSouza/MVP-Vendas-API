using Sales.Domain.Enums;

namespace Sales.Application.Dashboard.Common;

public sealed record DashboardResponse(
    int TotalSalesCount,
    int TodaySalesCount,
    int MonthSalesCount,
    decimal GrossRevenue,
    decimal EstimatedProfit,
    IReadOnlyList<PaymentMethodSalesResponse> SalesByPaymentMethod,
    IReadOnlyList<LowStockProductResponse> LowStockProducts,
    IReadOnlyList<RecentSaleResponse> RecentSales);

public sealed record PaymentMethodSalesResponse(
    PaymentMethod PaymentMethod,
    int Count,
    decimal Total);

public sealed record LowStockProductResponse(
    Guid Id,
    string Name,
    decimal StockQuantity);

public sealed record RecentSaleResponse(
    Guid Id,
    DateTime CreatedAt,
    PaymentMethod PaymentMethod,
    decimal Total,
    decimal Profit,
    int ItemCount);
