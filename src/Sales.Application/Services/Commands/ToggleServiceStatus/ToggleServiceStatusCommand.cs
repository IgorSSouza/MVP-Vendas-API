using MediatR;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Commands.ToggleServiceStatus;

public sealed class ToggleServiceStatusCommand : IRequest<ServiceResponse>
{
    public Guid Id { get; set; }
}
