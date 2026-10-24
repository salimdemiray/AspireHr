using Hr.Ozluk.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Hr.Ozluk.Infrastructure.Persistence;

public class OzlukDbContext
{
    private readonly IMongoDatabase _database;

    public OzlukDbContext(IOptions<MongoDbSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<Person> Persons => _database.GetCollection<Person>("persons");
    public IMongoCollection<PersonAddress> PersonAddresses => _database.GetCollection<PersonAddress>("person_addresses");
    public IMongoCollection<PersonContact> PersonContacts => _database.GetCollection<PersonContact>("person_contacts");
    public IMongoCollection<PersonFamily> PersonFamilies => _database.GetCollection<PersonFamily>("person_families");
}
