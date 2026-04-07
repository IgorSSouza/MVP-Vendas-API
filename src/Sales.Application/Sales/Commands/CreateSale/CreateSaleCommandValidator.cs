using FluentValidation;

namespace Sales.Application.Sales.Commands.CreateSale;

public sealed class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.PaymentMethod)
            .IsInEnum();

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Items)
            .NotEmpty();

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ItemType)
                    .IsInEnum();

                item.RuleFor(x => x.ItemId)
                    .NotEmpty();

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0);
            });
    }
}
