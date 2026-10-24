using Shared.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Hr.Ozluk.Domain.Entities;

public class PersonFamily : BaseEntity, IDateItem
{
    public string PersonId { get; set; } = string.Empty;
    public string RelationType { get; set; } = string.Empty; // Spouse, Child vb.
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? IdentityNumber { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; } = new DateOnly(1, 1, 1);
    public string? Note { get; set; }
}
