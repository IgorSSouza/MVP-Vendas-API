using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Queries.GetServiceById;

public sealed class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceResponse?>
{
    private readonly IAppDbContext _dbContext;

    public GetServiceByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponse?> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .Where(service => service.Id == request.Id)
            .Select(service => new ServiceResponse(
                service.Id,
                service.Name,
                service.Description,
                service.CostPrice,
                service.SalePrice,
                service.IsActive,
                service.CreatedAt,
                service.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
