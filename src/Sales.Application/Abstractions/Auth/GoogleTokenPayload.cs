namespace Sales.Application.Abstractions.Auth;

public sealed record GoogleTokenPayload(
    string Subject,
    string Email,
    string Name);
