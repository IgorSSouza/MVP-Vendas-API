namespace Sales.Application.Auth.Common;

public sealed record AuthUserResponse(
    Guid Id,
    string Name,
    string Email);

public sealed record AuthResponse(
    string? AccessToken,
    DateTime? ExpiresAt,
    AuthUserResponse User,
    Guid? CompanyId,
    string? CompanyName,
    bool NeedsCompanySetup);
