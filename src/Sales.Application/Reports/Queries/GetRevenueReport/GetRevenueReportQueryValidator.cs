using FluentValidation;

namespace Sales.Application.Reports.Queries.GetRevenueReport;

public sealed class GetRevenueReportQueryValidator : AbstractValidator<GetRevenueReportQuery>
{
    public GetRevenueReportQueryValidator()
    {
        RuleFor(x => x.StartDate)
            .NotNull();

        RuleFor(x => x.EndDate)
            .NotNull();

        RuleFor(x => x)
            .Must(x => x.StartDate <= x.EndDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("StartDate must be less than or equal to EndDate.");
    }
}
