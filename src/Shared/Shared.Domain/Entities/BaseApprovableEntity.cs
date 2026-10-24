using Shared.Domain.Enums;

namespace Shared.Domain.Entities;

public abstract class BaseApprovableEntity : BaseEntity
{
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Draft;

    public string? CurrentApproverId { get; set; }
    public string? CurrentApproverRoleId { get; set; }
    public string? CurrentApproverGroupId { get; set; }

    public int ApprovalStep { get; set; } = 0;
    public int TotalApprovalSteps { get; set; } = 1;

    public string? RequestNote { get; set; }
    public string? LastApprovalNote { get; set; }

    public DateTime? SubmittedAt { get; set; }
    public string? SubmittedBy { get; set; }

    public DateTime? ClosedAt { get; set; }
    public string? ClosedBy { get; set; }
}
