using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    private readonly IAppDbContext _dbContext;

    public GetProductByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .Where(product => product.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}
