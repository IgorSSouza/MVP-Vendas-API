using MediatR;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Commands.UpdateService;

public sealed class UpdateServiceCommand : IRequest<ServiceResponse>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal CostPrice { get; set; }

    public decimal SalePrice { get; set; }
}
