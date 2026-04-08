using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Commands.UpdateService;

public sealed class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, ServiceResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public UpdateServiceCommandHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<ServiceResponse> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        var service = await _dbContext.Services
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == companyId, cancellationToken);

        if (service is null)
        {
            throw new KeyNotFoundException("Service was not found.");
        }

        service.Name = request.Name.Trim();
        service.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        service.CostPrice = request.CostPrice;
        service.SalePrice = request.SalePrice;
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
