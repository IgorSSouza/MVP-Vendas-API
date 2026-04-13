using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Products.Common;
using Sales.Domain.Entities;

namespace Sales.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public CreateProductCommandHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");
        var barcode = NormalizeBarcode(request.Barcode);

        if (barcode is not null)
        {
            var barcodeExists = await _dbContext.Products
                .AnyAsync(x => x.CompanyId == companyId && x.Barcode == barcode, cancellationToken);

            if (barcodeExists)
            {
                throw new InvalidOperationException("A product with this barcode already exists for the current company.");
            }
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            Barcode = barcode,
            Name = request.Name.Trim(),
            Category = request.Category.Trim(),
            CostPrice = request.CostPrice,
            SalePrice = request.SalePrice,
            StockQuantity = request.StockQuantity,
            IsActive = request.IsActive,
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.Products.Add(product);
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
