using System.Text.Json.Serialization;
using MediatR;
using Sales.Application.Auth.Common;

namespace Sales.Application.Companies.Commands.SetupInitialCompany;

public sealed class SetupInitialCompanyCommand : IRequest<AuthResponse>
{
    [JsonIgnore]
    public Guid AuthenticatedUserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Cnpj { get; set; } = string.Empty;
}
