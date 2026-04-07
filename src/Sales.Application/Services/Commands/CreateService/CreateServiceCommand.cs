using MediatR;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Commands.CreateService;

public sealed class CreateServiceCommand : IRequest<ServiceResponse>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal CostPrice { get; set; }

    public decimal SalePrice { get; set; }

    public bool IsActive { get; set; } = true;
}
