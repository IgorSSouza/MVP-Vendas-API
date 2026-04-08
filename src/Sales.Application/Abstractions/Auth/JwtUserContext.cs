namespace Sales.Application.Abstractions.Auth;

public sealed record JwtUserContext(
    Guid UserId,
    string Email,
    Guid? CompanyId,
    bool NeedsCompanySetup);
