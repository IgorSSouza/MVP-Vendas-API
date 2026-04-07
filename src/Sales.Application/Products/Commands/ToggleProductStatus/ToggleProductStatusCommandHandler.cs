using MediatR;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Commands.ToggleProductStatus;

public sealed class ToggleProductStatusCommandHandler : IRequestHandler<ToggleProductStatusCommand, ProductResponse>
{
    private readonly IAppDbContext _dbContext;

    public ToggleProductStatusCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductResponse> Handle(ToggleProductStatusCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync(new object[] { request.Id }, cancellationToken);

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
