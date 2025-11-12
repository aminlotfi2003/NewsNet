using NewsNet.Domain.Entities;

namespace NewsNet.Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    Task<(string Token, DateTimeOffset ExpiresAt)> GenerateAsync(ApplicationUser user, CancellationToken cancellationToken = default);
}
