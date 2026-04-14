using MediatR;
using Sales.Application.Reports.Common;

namespace Sales.Application.Reports.Queries.GetRevenueReport;

public sealed class GetRevenueReportQuery : IRequest<RevenueReportResponse>
{
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
