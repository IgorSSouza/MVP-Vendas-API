using Sales.Domain.Enums;

namespace Sales.Application.Sales.Common;

public sealed record SaleSummaryResponse(
    Guid Id,
    DateTime CreatedAt,
    PaymentMethod PaymentMethod,
    decimal Subtotal,
    decimal Discount,
    decimal Total,
    decimal Profit,
    int ItemCount);

public sealed record SaleItemResponse(
    Guid Id,
    SaleItemType ItemType,
    Guid ItemId,
    string Name,
    decimal Quantity,
    decimal UnitCostPrice,
    decimal UnitSalePrice,
    decimal Subtotal,
    decimal Profit);

public sealed record SaleResponse(
    Guid Id,
    DateTime CreatedAt,
    PaymentMethod PaymentMethod,
    decimal Subtotal,
    decimal Discount,
    decimal Total,
    decimal Profit,
    IReadOnlyList<SaleItemResponse> Items);
