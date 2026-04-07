namespace Sales.Application.Products.Common;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Category,
    decimal CostPrice,
    decimal SalePrice,
    decimal StockQuantity,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
