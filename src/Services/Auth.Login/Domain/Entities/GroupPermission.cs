using Shared.Domain.Entities;

namespace Auth.Login.Domain.Entities;

public class GroupPermission : BaseEntity
{
    public string GroupId { get; set; } = string.Empty;
    public string PermissionId { get; set; } = string.Empty;
}
