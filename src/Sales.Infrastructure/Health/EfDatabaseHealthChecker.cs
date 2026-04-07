using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Health;
using Sales.Infrastructure.Persistence;

namespace Sales.Infrastructure.Health;

public sealed class EfDatabaseHealthChecker : IDatabaseHealthChecker
{
    private readonly AppDbContext _dbContext;

    public EfDatabaseHealthChecker(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.CanConnectAsync(cancellationToken);
    }
}
