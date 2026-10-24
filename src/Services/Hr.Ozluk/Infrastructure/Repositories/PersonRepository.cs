using Hr.Ozluk.Domain.Entities;
using Hr.Ozluk.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Hr.Ozluk.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly OzlukDbContext _context;

    public PersonRepository(OzlukDbContext context)
    {
        _context = context;
    }

    public async Task<List<Person>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _context.Persons.Find(Builders<Person>.Filter.Empty).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<Person?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Person>.Filter.Eq(x => x.Id, id);
        return await _context.Persons.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Person> CreateAsync(Person person, CancellationToken cancellationToken = default)
    {
        person.CreatedAt = DateTime.UtcNow;
        await _context.Persons.InsertOneAsync(person, cancellationToken: cancellationToken);
        return person;
    }

    public async Task<Person?> UpdateAsync(Person person, CancellationToken cancellationToken = default)
    {
        person.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<Person>.Filter.Eq(x => x.Id, person.Id);
        var result = await _context.Persons.ReplaceOneAsync(filter, person, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? person : null;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Person>.Filter.Eq(x => x.Id, id);
        var result = await _context.Persons.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }
}
