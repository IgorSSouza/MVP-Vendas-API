using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Sales.Common;
using Sales.Domain.Entities;
using Sales.Domain.Enums;

namespace Sales.Application.Sales.Commands.CreateSale;

public sealed class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public CreateSaleCommandHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<SaleResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var now = DateTime.Now;
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            PaymentMethod = request.PaymentMethod,
            Discount = request.Discount,
            CreatedAt = now
        };

        var items = new List<SaleItem>();

        foreach (var item in request.Items)
        {
            if (item.ItemType == SaleItemType.Product)
            {
                var product = await _dbContext.Products
                    .FirstOrDefaultAsync(x => x.Id == item.ItemId && x.CompanyId == companyId, cancellationToken);

                if (product is null)
                {
                    throw new KeyNotFoundException("Product was not found.");
                }

                if (!product.IsActive)
                {
                    throw new InvalidOperationException("Product is inactive.");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'.");
                }

                product.StockQuantity -= item.Quantity;
                product.UpdatedAt = now;

                items.Add(CreateItem(
                    sale.Id,
                    SaleItemType.Product,
                    product.Id,
                    product.Name,
                    item.Quantity,
                    product.CostPrice,
                    product.SalePrice));
            }
            else if (item.ItemType == SaleItemType.Service)
            {
                var service = await _dbContext.Services
                    .FirstOrDefaultAsync(x => x.Id == item.ItemId && x.CompanyId == companyId, cancellationToken);

                if (service is null)
                {
                    throw new KeyNotFoundException("Service was not found.");
                }

                if (!service.IsActive)
                {
                    throw new InvalidOperationException("Service is inactive.");
                }

                items.Add(CreateItem(
                    sale.Id,
                    SaleItemType.Service,
                    service.Id,
                    service.Name,
                    item.Quantity,
                    service.CostPrice,
                    service.SalePrice));
            }
            else
            {
                throw new InvalidOperationException("Invalid sale item type.");
            }
        }

        sale.Items = items;
        sale.Subtotal = items.Sum(item => item.Subtotal);
        sale.Total = sale.Subtotal - request.Discount;

        if (sale.Total < 0)
        {
            throw new InvalidOperationException("Sale total cannot be negative.");
        }

        sale.Profit = items.Sum(item => item.Profit) - request.Discount;

        _dbContext.Sales.Add(sale);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return MapSale(sale);
    }

    private static SaleItem CreateItem(
        Guid saleId,
        SaleItemType itemType,
        Guid itemId,
        string name,
        decimal quantity,
        decimal unitCostPrice,
        decimal unitSalePrice)
    {
        var subtotal = quantity * unitSalePrice;
        var profit = (unitSalePrice - unitCostPrice) * quantity;

        return new SaleItem
        {
            Id = Guid.NewGuid(),
            SaleId = saleId,
            ItemType = itemType,
            ItemId = itemId,
            Name = name,
            Quantity = quantity,
            UnitCostPrice = unitCostPrice,
            UnitSalePrice = unitSalePrice,
            Subtotal = subtotal,
            Profit = profit
        };
    }

    private static SaleResponse MapSale(Sale sale)
    {
        return new SaleResponse(
            sale.Id,
            sale.CreatedAt,
            sale.PaymentMethod,
            sale.Subtotal,
            sale.Discount,
            sale.Total,
            sale.Profit,
            sale.Items
                .Select(item => new SaleItemResponse(
                    item.Id,
                    item.ItemType,
                    item.ItemId,
                    item.Name,
                    item.Quantity,
                    item.UnitCostPrice,
                    item.UnitSalePrice,
                    item.Subtotal,
                    item.Profit))
                .ToList());
    }
}
