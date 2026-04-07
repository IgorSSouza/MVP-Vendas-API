using MediatR;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Commands.ToggleProductStatus;

public sealed class ToggleProductStatusCommand : IRequest<ProductResponse>
{
    public Guid Id { get; set; }
}
