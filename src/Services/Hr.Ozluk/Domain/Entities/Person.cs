using Hr.Ozluk.Domain.Enums;
using Shared.Domain.Entities;

namespace Hr.Ozluk.Domain.Entities;

public class Person : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? IdentityNumber { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public Gender? Gender { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? Nationality { get; set; }
    public string? Note { get; set; }
}
