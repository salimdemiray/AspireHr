namespace Shared.Domain.Interfaces;

public interface IDateItem
{
    DateOnly StartDate { get; set; }
    DateOnly EndDate { get; set; }
}
