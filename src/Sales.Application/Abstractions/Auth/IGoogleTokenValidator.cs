namespace Sales.Application.Abstractions.Auth;

public interface IGoogleTokenValidator
{
    Task<GoogleTokenPayload> ValidateAsync(string idToken, CancellationToken cancellationToken = default);
}
