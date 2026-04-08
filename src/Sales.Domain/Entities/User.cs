using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string GoogleSubject { get; set; } = string.Empty;
    public AuthProvider AuthProvider { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
