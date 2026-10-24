using Auth.Login.Domain.Entities;
using Auth.Login.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Auth.Login.Infrastructure.Repositories;

public class ApplicationUserRepository : IApplicationUserRepository
{
    private readonly AuthDbContext _context;

    public ApplicationUserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(x => x.Username, username);
        return await _context.Users.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(x => x.Id, id);
        return await _context.Users.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}
