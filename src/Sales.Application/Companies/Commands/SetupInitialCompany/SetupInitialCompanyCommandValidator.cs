using FluentValidation;

namespace Sales.Application.Companies.Commands.SetupInitialCompany;

public sealed class SetupInitialCompanyCommandValidator : AbstractValidator<SetupInitialCompanyCommand>
{
    public SetupInitialCompanyCommandValidator()
    {
        RuleFor(x => x.AuthenticatedUserId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .Must(BeValidCnpjFormat)
            .WithMessage("Cnpj must contain 14 digits.");
    }

    private static bool BeValidCnpjFormat(string cnpj)
    {
        var digits = new string(cnpj.Where(char.IsDigit).ToArray());
        return digits.Length == 14;
    }
}
