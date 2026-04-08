using System.Security.Claims;
using Sales.Application.Abstractions.Auth;

namespace Sales.Api.Auth;

public sealed class HttpCurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId => TryParseGuid("userId") ?? TryParseGuid(ClaimTypes.NameIdentifier);

    public Guid? CompanyId => TryParseGuid("companyId");

    public string? Email => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
        ?? _httpContextAccessor.HttpContext?.User.FindFirstValue("email")
        ?? _httpContextAccessor.HttpContext?.User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

    public bool NeedsCompanySetup =>
        bool.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue("needsCompanySetup"), out var value) && value;

    private Guid? TryParseGuid(string claimType)
    {
        var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
        return Guid.TryParse(value, out var guid) ? guid : null;
    }
}
