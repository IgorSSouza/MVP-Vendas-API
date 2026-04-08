using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Auth.Common;
using Sales.Domain.Entities;
using Sales.Domain.Enums;

namespace Sales.Application.Companies.Commands.SetupInitialCompany;

public sealed class SetupInitialCompanyCommandHandler : IRequestHandler<SetupInitialCompanyCommand, AuthResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly IAccessTokenService _accessTokenService;

    public SetupInitialCompanyCommandHandler(
        IAppDbContext dbContext,
        IAccessTokenService accessTokenService)
    {
        _dbContext = dbContext;
        _accessTokenService = accessTokenService;
    }

    public async Task<AuthResponse> Handle(SetupInitialCompanyCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == request.AuthenticatedUserId, cancellationToken);

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Authenticated user was not found or is inactive.");
        }

        var alreadyLinked = await _dbContext.UserCompanies
            .AnyAsync(x => x.UserId == request.AuthenticatedUserId, cancellationToken);

        if (alreadyLinked)
        {
            throw new InvalidOperationException("Initial company setup has already been completed for this user.");
        }

        var normalizedCnpj = NormalizeCnpj(request.Cnpj);

        var companyExists = await _dbContext.Companies
            .AnyAsync(x => x.Cnpj == normalizedCnpj, cancellationToken);

        if (companyExists)
        {
            throw new InvalidOperationException("A company with this CNPJ already exists.");
        }

        var now = DateTime.Now;
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Cnpj = normalizedCnpj,
            IsActive = true,
            CreatedAt = now
        };

        var userCompany = new UserCompany
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CompanyId = company.Id,
            Role = UserCompanyRole.Owner,
            CreatedAt = now
        };

        _dbContext.Companies.Add(company);
        _dbContext.UserCompanies.Add(userCompany);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var accessToken = _accessTokenService.Generate(new JwtUserContext(
            user.Id,
            user.Email,
            company.Id,
            false));

        return new AuthResponse(
            accessToken.AccessToken,
            accessToken.ExpiresAt,
            new AuthUserResponse(user.Id, user.Name, user.Email),
            company.Id,
            company.Name,
            false);
    }

    private static string NormalizeCnpj(string cnpj)
    {
        return new string(cnpj.Where(char.IsDigit).ToArray());
    }
}
