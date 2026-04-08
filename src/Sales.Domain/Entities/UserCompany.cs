using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public sealed class UserCompany
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public UserCompanyRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
