using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IReadOnlyList<ProductResponse>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllProductsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Products
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
