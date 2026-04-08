namespace Sales.Application.Abstractions.Auth;

public interface IAccessTokenService
{
    AccessTokenResult Generate(JwtUserContext context);
}
