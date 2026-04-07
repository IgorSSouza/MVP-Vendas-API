using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Queries.GetAllServices;

public sealed class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, IReadOnlyList<ServiceResponse>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllServicesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ServiceResponse>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .OrderBy(service => service.Name)
            .Select(service => new ServiceResponse(
                service.Id,
                service.Name,
                service.Description,
                service.CostPrice,
                service.SalePrice,
                service.IsActive,
                service.CreatedAt,
                service.UpdatedAt))
            .ToListAsync(cancellationToken);
    }
}
