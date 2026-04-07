using FluentValidation;

namespace Sales.Application.Sales.Queries.GetSaleById;

public sealed class GetSaleByIdQueryValidator : AbstractValidator<GetSaleByIdQuery>
{
    public GetSaleByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
