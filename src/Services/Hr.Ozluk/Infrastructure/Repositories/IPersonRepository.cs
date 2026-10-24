using Hr.Ozluk.Domain.Entities;

namespace Hr.Ozluk.Infrastructure.Repositories;

public interface IPersonRepository
{
    Task<List<Person>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Person?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Person> CreateAsync(Person person, CancellationToken cancellationToken = default);
    Task<Person?> UpdateAsync(Person person, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
