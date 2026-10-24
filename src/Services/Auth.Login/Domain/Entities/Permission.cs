using Auth.Login.Domain.Enums;
using Shared.Domain.Entities;

namespace Auth.Login.Domain.Entities;

public class Permission : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public PermissionType PermissionType { get; set; } = PermissionType.Screen;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
