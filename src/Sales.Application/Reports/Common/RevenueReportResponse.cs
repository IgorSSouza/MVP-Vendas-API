using Sales.Domain.Enums;

namespace Sales.Application.Reports.Common;

public sealed record RevenuePaymentMethodBreakdownResponse(
    PaymentMethod PaymentMethod,
    int SalesCount,
    decimal SubtotalAmount,
    decimal TotalAmount,
    decimal ProfitAmount);

public sealed record RevenueReportResponse(
    DateTime StartDate,
    DateTime EndDate,
    int SalesCount,
    decimal SubtotalAmount,
    decimal TotalAmount,
    decimal ProfitAmount,
    IReadOnlyList<RevenuePaymentMethodBreakdownResponse> PaymentMethodBreakdown);
