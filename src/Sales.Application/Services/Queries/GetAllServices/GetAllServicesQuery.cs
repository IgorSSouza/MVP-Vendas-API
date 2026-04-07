using MediatR;
using Sales.Application.Services.Common;

namespace Sales.Application.Services.Queries.GetAllServices;

public sealed class GetAllServicesQuery : IRequest<IReadOnlyList<ServiceResponse>>
{
}
