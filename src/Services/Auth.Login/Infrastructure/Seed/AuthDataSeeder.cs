using Auth.Login.Domain.Entities;
using Auth.Login.Domain.Enums;
using Auth.Login.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Auth.Login.Infrastructure.Seed;

public class AuthDataSeeder
{
    private readonly AuthDbContext _context;

    public AuthDataSeeder(AuthDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Admin kullanıcı
        var adminExists = await _context.Users.Find(Builders<ApplicationUser>.Filter.Eq(x => x.Username, "admin")).AnyAsync();
        if (!adminExists)
        {
            var admin = new ApplicationUser
            {
                Username = "admin",
                Email = "admin@aspirehr.local",
                FullName = "Sistem Yöneticisi",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                IsActive = true,
                MustChangePassword = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };
            await _context.Users.InsertOneAsync(admin);

            // Admin grubu
            var adminGroup = new AuthGroup
            {
                Code = "ADMIN",
                Name = "Yönetici",
                Description = "Tam yetkili grup",
                IsActive = true,
                SortOrder = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };
            await _context.Groups.InsertOneAsync(adminGroup);

            // Kullanıcı-Grup
            await _context.UserGroups.InsertOneAsync(new UserGroup
            {
                UserId = admin.Id,
                GroupId = adminGroup.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Temel yetkiler
            var permissions = new List<Permission>
            {
                new() { Code = "AUTH_MANAGE", Name = "Kullanıcı/Grup/Yetki Yönetimi", Module = "AUTH", PermissionType = PermissionType.Screen, IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
                new() { Code = "OZLUK_PERSON_LIST", Name = "Kişi Listeleme", Module = "OZLUK", PermissionType = PermissionType.Screen, IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
                new() { Code = "OZLUK_PERSON_CREATE", Name = "Kişi Ekleme", Module = "OZLUK", PermissionType = PermissionType.Action, IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
                new() { Code = "OZLUK_PERSON_UPDATE", Name = "Kişi Güncelleme", Module = "OZLUK", PermissionType = PermissionType.Action, IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" }
            };
            await _context.Permissions.InsertManyAsync(permissions);

            // Grup-Yetki
            foreach (var p in permissions)
            {
                await _context.GroupPermissions.InsertOneAsync(new GroupPermission
                {
                    GroupId = adminGroup.Id,
                    PermissionId = p.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                });
            }
        }
    }
}
