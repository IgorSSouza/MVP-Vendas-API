using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Queries.GetServiceById;

public sealed class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceResponse?>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public GetServiceByIdQueryHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<ServiceResponse?> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        return await _dbContext.Services
            .Where(service => service.Id == request.Id && service.CompanyId == companyId)
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
