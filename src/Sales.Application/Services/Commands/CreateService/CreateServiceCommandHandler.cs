using MediatR;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;
using Sales.Domain.Entities;

namespace Sales.Application.Services.Commands.CreateService;

public sealed class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ServiceResponse>
{
    private readonly IAppDbContext _dbContext;

    public CreateServiceCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponse> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;

        var service = new Service
        {
            Id = Guid.NewGuid(),
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
