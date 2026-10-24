using Auth.Login.Domain.Entities;

namespace Auth.Login.Infrastructure.Repositories;

public interface IApplicationUserRepository
{
    Task<ApplicationUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
