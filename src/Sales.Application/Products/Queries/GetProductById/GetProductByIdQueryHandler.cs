using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public GetProductByIdQueryHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        return await _dbContext.Products
            .Where(product => product.Id == request.Id && product.CompanyId == companyId)
            .Select(product => new ProductResponse(
                product.Id,
                product.Barcode,
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
