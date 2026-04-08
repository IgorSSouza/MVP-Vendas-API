using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sales.Application.Abstractions.Auth;

namespace Sales.Infrastructure.Auth;

public sealed class JwtAccessTokenService : IAccessTokenService
{
    private readonly JwtOptions _options;

    public JwtAccessTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public AccessTokenResult Generate(JwtUserContext context)
    {
        var now = DateTime.UtcNow;
        var expiresAt = now.AddMinutes(_options.ExpiresInMinutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, context.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, context.Email),
            new("userId", context.UserId.ToString()),
            new("needsCompanySetup", context.NeedsCompanySetup.ToString().ToLowerInvariant())
        };

        if (context.CompanyId.HasValue)
        {
            claims.Add(new Claim("companyId", context.CompanyId.Value.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AccessTokenResult(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAt);
    }
}
