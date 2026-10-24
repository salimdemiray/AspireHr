using Shared.Domain.Enums;

namespace Shared.Domain.Entities;

public class ApprovalHistory : BaseEntity
{
    public string EntityId { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;

    public ApprovalStatus FromStatus { get; set; }
    public ApprovalStatus ToStatus { get; set; }

    public string ActionBy { get; set; } = string.Empty;
    public string? ActionByRoleId { get; set; }
    public string? ActionByGroupId { get; set; }

    public int Step { get; set; }
    public string? Comment { get; set; }
    public DateTime ActionAt { get; set; } = DateTime.UtcNow;
}
