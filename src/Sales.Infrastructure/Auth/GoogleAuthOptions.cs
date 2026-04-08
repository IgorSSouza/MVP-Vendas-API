namespace Sales.Infrastructure.Auth;

public sealed class GoogleAuthOptions
{
    public const string SectionName = "GoogleAuth";

    public List<string> AllowedAudiences { get; set; } = [];
}
