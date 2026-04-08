using MediatR;
using Sales.Application.Auth.Common;

namespace Sales.Application.Auth.Commands.AuthenticateWithGoogle;

public sealed class AuthenticateWithGoogleCommand : IRequest<AuthResponse>
{
    public string IdToken { get; set; } = string.Empty;
}
