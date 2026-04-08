using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Commands.ToggleServiceStatus;

public sealed class ToggleServiceStatusCommandHandler : IRequestHandler<ToggleServiceStatusCommand, ServiceResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public ToggleServiceStatusCommandHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<ServiceResponse> Handle(ToggleServiceStatusCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        var service = await _dbContext.Services
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == companyId, cancellationToken);

        if (service is null)
        {
            throw new KeyNotFoundException("Service was not found.");
        }

        service.IsActive = !service.IsActive;
        service.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ServiceResponse(
            service.Id,
            service.Name,
            service.Description,
            service.CostPrice,
            service.SalePrice,
            service.IsActive,
            service.CreatedAt,
            service.UpdatedAt);
    }
}
