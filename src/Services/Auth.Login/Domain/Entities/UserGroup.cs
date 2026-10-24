using Shared.Domain.Entities;

namespace Auth.Login.Domain.Entities;

public class UserGroup : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
}
