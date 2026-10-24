using Shared.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Hr.Ozluk.Domain.Entities;

public class PersonContact : BaseEntity, IDateItem
{
    public string PersonId { get; set; } = string.Empty;
    public string ContactType { get; set; } = string.Empty; // Phone, Email vb.
    public string Value { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; } = new DateOnly(1, 1, 1);
}
