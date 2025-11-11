using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsNet.Domain.Entities;
using NewsNet.Domain.Enums;
using NewsNet.Infrastructure.Security;

namespace NewsNet.Infrastructure.Persistence.Seed;

public sealed class DatabaseInitializer
{
    private readonly NewsNetDbContext _db;
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly Pbkdf2PasswordHasher _hasher;

    public DatabaseInitializer(NewsNetDbContext db, ILogger<DatabaseInitializer> logger, Pbkdf2PasswordHasher hasher)
    {
        _db = db;
        _logger = logger;
        _hasher = hasher;
    }

    public async Task InitializeAsync()
    {
        await _db.Database.MigrateAsync();

        var adminEmail = "admin@example.com";
        var admin = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == adminEmail);

        if (admin is null)
        {
            var now = DateTimeOffset.UtcNow;
            var passwordHash = _hasher.Hash("ChangeMe!PleaseSetStrongPassword#2025");

            var newAdmin = User.Create(adminEmail, passwordHash, UserRole.Admin, now);
            _db.Users.Add(newAdmin);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Seeded Admin user with email {Email}", adminEmail);
        }
    }
}
