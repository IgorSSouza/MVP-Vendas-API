namespace Sales.Application.Services.Common;

public sealed record ServiceResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal CostPrice,
    decimal SalePrice,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
