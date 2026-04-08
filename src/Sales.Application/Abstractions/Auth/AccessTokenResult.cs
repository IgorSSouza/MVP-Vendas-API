namespace Sales.Application.Abstractions.Auth;

public sealed record AccessTokenResult(
    string AccessToken,
    DateTime ExpiresAt);
