namespace Sales.Application.Abstractions.Auth;

public interface ICurrentUserContext
{
    Guid? UserId { get; }

    Guid? CompanyId { get; }

    string? Email { get; }

    bool NeedsCompanySetup { get; }
}
