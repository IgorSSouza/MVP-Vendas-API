using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Commands.ToggleProductStatus;

public sealed class ToggleProductStatusCommandHandler : IRequestHandler<ToggleProductStatusCommand, ProductResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public ToggleProductStatusCommandHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<ProductResponse> Handle(ToggleProductStatusCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == companyId, cancellationToken);

        if (product is null)
        {
            throw new KeyNotFoundException("Product was not found.");
        }

        product.IsActive = !product.IsActive;
        product.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ProductResponse(
            product.Id,
            product.Name,
            product.Category,
            product.CostPrice,
            product.SalePrice,
            product.StockQuantity,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);
    }
}
