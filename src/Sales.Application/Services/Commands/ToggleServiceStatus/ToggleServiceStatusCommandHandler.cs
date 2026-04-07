using MediatR;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Commands.ToggleServiceStatus;

public sealed class ToggleServiceStatusCommandHandler : IRequestHandler<ToggleServiceStatusCommand, ServiceResponse>
{
    private readonly IAppDbContext _dbContext;

    public ToggleServiceStatusCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponse> Handle(ToggleServiceStatusCommand request, CancellationToken cancellationToken)
    {
        var service = await _dbContext.Services.FindAsync(new object[] { request.Id }, cancellationToken);

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
