using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IReadOnlyList<ProductResponse>>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public GetAllProductsQueryHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<IReadOnlyList<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        return await _dbContext.Products
            .Where(product => product.CompanyId == companyId)
            .OrderBy(product => product.Name)
            .Select(product => new ProductResponse(
                product.Id,
                product.Name,
                product.Category,
                product.CostPrice,
                product.SalePrice,
                product.StockQuantity,
                product.IsActive,
                product.CreatedAt,
                product.UpdatedAt))
            .ToListAsync(cancellationToken);
    }
}
