using FluentValidation;

namespace Sales.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Barcode)
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Category)
            .NotEmpty();

        RuleFor(x => x.CostPrice)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.SalePrice)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0);
    }
}
