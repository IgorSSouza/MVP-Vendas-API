using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Auth.Common;
using Sales.Domain.Entities;
using Sales.Domain.Enums;

namespace Sales.Application.Auth.Commands.AuthenticateWithGoogle;

public sealed class AuthenticateWithGoogleCommandHandler : IRequestHandler<AuthenticateWithGoogleCommand, AuthResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly IGoogleTokenValidator _googleTokenValidator;
    private readonly IAccessTokenService _accessTokenService;

    public AuthenticateWithGoogleCommandHandler(
        IAppDbContext dbContext,
        IGoogleTokenValidator googleTokenValidator,
        IAccessTokenService accessTokenService)
    {
        _dbContext = dbContext;
        _googleTokenValidator = googleTokenValidator;
        _accessTokenService = accessTokenService;
    }

    public async Task<AuthResponse> Handle(AuthenticateWithGoogleCommand request, CancellationToken cancellationToken)
    {
        var payload = await _googleTokenValidator.ValidateAsync(request.IdToken, cancellationToken);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.GoogleSubject == payload.Subject, cancellationToken);

        if (user is null)
        {
            user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email == payload.Email, cancellationToken);
        }

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Name = payload.Name.Trim(),
                Email = payload.Email.Trim(),
                GoogleSubject = payload.Subject,
                AuthProvider = AuthProvider.Google,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _dbContext.Users.Add(user);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(user.GoogleSubject) &&
                !string.Equals(user.GoogleSubject, payload.Subject, StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException("This email is already linked to another Google account.");
            }

            user.Name = payload.Name.Trim();
            user.Email = payload.Email.Trim();

            if (string.IsNullOrWhiteSpace(user.GoogleSubject))
            {
                user.GoogleSubject = payload.Subject;
            }

            user.AuthProvider = AuthProvider.Google;
            user.IsActive = true;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var userCompany = await _dbContext.UserCompanies
            .AsNoTracking()
            .Where(link => link.UserId == user.Id)
            .Join(
                _dbContext.Companies.AsNoTracking().Where(company => company.IsActive),
                userLink => userLink.CompanyId,
                company => company.Id,
                (userLink, company) => new
                {
                    userLink.CompanyId,
                    CompanyName = company.Name,
                    userLink.CreatedAt
                })
            .OrderBy(link => link.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var authUser = new AuthUserResponse(user.Id, user.Name, user.Email);

        if (userCompany is null)
        {
            var onboardingToken = _accessTokenService.Generate(new JwtUserContext(
                user.Id,
                user.Email,
                null,
                true));

            return new AuthResponse(
                AccessToken: onboardingToken.AccessToken,
                ExpiresAt: onboardingToken.ExpiresAt,
                User: authUser,
                CompanyId: null,
                CompanyName: null,
                NeedsCompanySetup: true);
        }

        var accessToken = _accessTokenService.Generate(new JwtUserContext(
            user.Id,
            user.Email,
            userCompany.CompanyId,
            false));

        return new AuthResponse(
            accessToken.AccessToken,
            accessToken.ExpiresAt,
            authUser,
            userCompany.CompanyId,
            userCompany.CompanyName,
            false);
    }
}
