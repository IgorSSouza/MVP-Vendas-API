using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public UpdateProductCommandHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");
        var barcode = NormalizeBarcode(request.Barcode);

        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == companyId, cancellationToken);

        if (product is null)
        {
            throw new KeyNotFoundException("Product was not found.");
        }

        if (barcode is not null)
        {
            var barcodeExists = await _dbContext.Products
                .AnyAsync(x => x.CompanyId == companyId && x.Barcode == barcode && x.Id != request.Id, cancellationToken);

            if (barcodeExists)
            {
                throw new InvalidOperationException("A product with this barcode already exists for the current company.");
            }
        }

        product.Barcode = barcode;
        product.Name = request.Name.Trim();
        product.Category = request.Category.Trim();
        product.CostPrice = request.CostPrice;
        product.SalePrice = request.SalePrice;
        product.StockQuantity = request.StockQuantity;
        product.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ProductResponse(
            product.Id,
            product.Barcode,
            product.Name,
            product.Category,
            product.CostPrice,
            product.SalePrice,
            product.StockQuantity,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);
    }

    private static string? NormalizeBarcode(string? barcode)
    {
        return string.IsNullOrWhiteSpace(barcode) ? null : barcode.Trim();
    }
}
