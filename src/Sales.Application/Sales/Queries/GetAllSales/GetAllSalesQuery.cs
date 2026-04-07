using MediatR;
using Sales.Application.Sales.Common;

namespace Sales.Application.Sales.Queries.GetAllSales;

public sealed class GetAllSalesQuery : IRequest<IReadOnlyList<SaleSummaryResponse>>
{
}
