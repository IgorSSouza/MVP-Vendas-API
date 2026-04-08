using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Sales.Application.Abstractions.Auth;

namespace Sales.Infrastructure.Auth;

public sealed class GoogleIdTokenValidator : IGoogleTokenValidator
{
    private readonly GoogleAuthOptions _options;

    public GoogleIdTokenValidator(IOptions<GoogleAuthOptions> options)
    {
        _options = options.Value;
    }

    public async Task<GoogleTokenPayload> ValidateAsync(string idToken, CancellationToken cancellationToken = default)
    {
        if (_options.AllowedAudiences.Count == 0)
        {
            throw new InvalidOperationException("GoogleAuth:AllowedAudiences is not configured.");
        }

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = _options.AllowedAudiences
                });

            return new GoogleTokenPayload(
                payload.Subject,
                payload.Email,
                payload.Name);
        }
        catch (InvalidJwtException ex)
        {
            throw new UnauthorizedAccessException($"Invalid Google token: {ex.Message}");
        }
    }
}
