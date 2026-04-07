using FluentValidation;

namespace Sales.Application.Products.Commands.ToggleProductStatus;

public sealed class ToggleProductStatusCommandValidator : AbstractValidator<ToggleProductStatusCommand>
{
    public ToggleProductStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
