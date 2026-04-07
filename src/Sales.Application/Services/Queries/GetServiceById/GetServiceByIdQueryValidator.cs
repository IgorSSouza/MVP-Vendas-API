using FluentValidation;

namespace Sales.Application.Services.Queries.GetServiceById;

public sealed class GetServiceByIdQueryValidator : AbstractValidator<GetServiceByIdQuery>
{
    public GetServiceByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
