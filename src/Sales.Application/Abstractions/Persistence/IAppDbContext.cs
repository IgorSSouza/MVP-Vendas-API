using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Sales.Application.Abstractions.Persistence;

public interface IAppDbContext
{
    DatabaseFacade Database { get; }

    DbSet<Product> Products { get; }

    DbSet<Service> Services { get; }

    DbSet<Sale> Sales { get; }

    DbSet<SaleItem> SaleItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
