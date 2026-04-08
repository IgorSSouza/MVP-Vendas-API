using FluentValidation;

namespace Sales.Application.Auth.Commands.AuthenticateWithGoogle;

public sealed class AuthenticateWithGoogleCommandValidator : AbstractValidator<AuthenticateWithGoogleCommand>
{
    public AuthenticateWithGoogleCommandValidator()
    {
        RuleFor(x => x.IdToken)
            .NotEmpty();
    }
}
