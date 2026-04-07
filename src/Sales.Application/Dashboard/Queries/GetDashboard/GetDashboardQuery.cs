using MediatR;
using Sales.Application.Dashboard.Common;

namespace Sales.Application.Dashboard.Queries.GetDashboard;

public sealed class GetDashboardQuery : IRequest<DashboardResponse>
{
}
