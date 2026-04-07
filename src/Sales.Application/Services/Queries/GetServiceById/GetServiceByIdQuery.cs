using MediatR;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Queries.GetServiceById;

public sealed class GetServiceByIdQuery : IRequest<ServiceResponse?>
{
    public Guid Id { get; set; }
}
