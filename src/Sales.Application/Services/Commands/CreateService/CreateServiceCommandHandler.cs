using MediatR;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;
using Sales.Domain.Entities;

namespace Sales.Application.Services.Commands.CreateService;

public sealed class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ServiceResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public CreateServiceCommandHandler(IAppDbContext dbContext, ICurrentUserContext currentUserContext)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<ServiceResponse> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var companyId = _currentUserContext.CompanyId
            ?? throw new UnauthorizedAccessException("A company context is required.");

        var service = new Service
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            Name = request.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            CostPrice = request.CostPrice,
            SalePrice = request.SalePrice,
            IsActive = request.IsActive,
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.Services.Add(service);
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
