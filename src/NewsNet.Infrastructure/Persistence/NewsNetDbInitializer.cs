using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsNet.Domain.Entities;

namespace NewsNet.Infrastructure.Persistence;

public sealed class NewsNetDbInitializer
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<NewsNetDbInitializer> _logger;
    private readonly NewsNetDbContext _context;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public NewsNetDbInitializer(
        IConfiguration configuration,
        ILogger<NewsNetDbInitializer> logger,
        NewsNetDbContext context,
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _logger = logger;
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task InitialiseAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.MigrateAsync(cancellationToken);
        await EnsureRolesAsync(cancellationToken);
        await EnsureAdminUserAsync(cancellationToken);
    }

    private async Task EnsureRolesAsync(CancellationToken cancellationToken)
    {
        const string adminRoleName = "Admin";

        if (await _roleManager.RoleExistsAsync(adminRoleName))
            return;

        var role = new ApplicationRole
        {
            Name = adminRoleName,
            NormalizedName = adminRoleName.ToUpperInvariant(),
            Description = "System administrator"
        };

        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(error => error.Description));
            _logger.LogError("Failed to create admin role: {Errors}", errors);
            throw new InvalidOperationException("Failed to create admin role.");
        }
    }

    private async Task EnsureAdminUserAsync(CancellationToken cancellationToken)
    {
        var adminSection = _configuration.GetSection("Admin");
        var email = adminSection.GetValue<string>("Email");
        var password = adminSection.GetValue<string>("Password");

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Admin user configuration is missing. Skipping admin user provisioning.");
            return;
        }

        var adminUser = await _userManager.FindByEmailAsync(email);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var createResult = await _userManager.CreateAsync(adminUser, password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(error => error.Description));
                _logger.LogError("Failed to create admin user: {Errors}", errors);
                throw new InvalidOperationException("Failed to create admin user.");
            }
        }

        if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, "Admin");

            if (!addToRoleResult.Succeeded)
            {
                var errors = string.Join(", ", addToRoleResult.Errors.Select(error => error.Description));
                _logger.LogError("Failed to assign admin role: {Errors}", errors);
                throw new InvalidOperationException("Failed to assign admin role to user.");
            }
        }
    }
}
