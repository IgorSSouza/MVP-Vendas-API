using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Queries.GetAllServices;

public sealed class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, IReadOnlyList<ServiceResponse>>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public GetAllServicesQueryHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<IReadOnlyList<ServiceResponse>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        return await _dbContext.Services
            .Where(service => service.CompanyId == companyId)
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
