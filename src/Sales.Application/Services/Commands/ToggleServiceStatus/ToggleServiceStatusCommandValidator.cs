using FluentValidation;

namespace Sales.Application.Services.Commands.ToggleServiceStatus;

public sealed class ToggleServiceStatusCommandValidator : AbstractValidator<ToggleServiceStatusCommand>
{
    public ToggleServiceStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
