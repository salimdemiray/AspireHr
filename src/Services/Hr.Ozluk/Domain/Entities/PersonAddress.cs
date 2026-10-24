using Shared.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Hr.Ozluk.Domain.Entities;

public class PersonAddress : BaseEntity, IDateItem
{
    public string PersonId { get; set; } = string.Empty;
    public string AddressType { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Neighborhood { get; set; }
    public string? PostalCode { get; set; }
    public string? AddressLine { get; set; }
    public bool IsPrimary { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; } = new DateOnly(1, 1, 1);
}
