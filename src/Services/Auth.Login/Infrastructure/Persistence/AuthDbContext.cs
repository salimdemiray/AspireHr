using Auth.Login.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Auth.Login.Infrastructure.Persistence;

public class AuthDbContext
{
    private readonly IMongoDatabase _database;

    public AuthDbContext(IOptions<MongoDbSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<ApplicationUser> Users => _database.GetCollection<ApplicationUser>("auth_users");
    public IMongoCollection<AuthGroup> Groups => _database.GetCollection<AuthGroup>("auth_groups");
    public IMongoCollection<UserGroup> UserGroups => _database.GetCollection<UserGroup>("auth_user_groups");
    public IMongoCollection<Permission> Permissions => _database.GetCollection<Permission>("auth_permissions");
    public IMongoCollection<GroupPermission> GroupPermissions => _database.GetCollection<GroupPermission>("auth_group_permissions");
    public IMongoCollection<UserPermission> UserPermissions => _database.GetCollection<UserPermission>("auth_user_permissions");
}
