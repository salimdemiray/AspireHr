using Shared.Domain.Entities;

namespace Auth.Login.Domain.Entities;

public class UserPermission : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string PermissionId { get; set; } = string.Empty;
    public bool IsGranted { get; set; } = true;
}
