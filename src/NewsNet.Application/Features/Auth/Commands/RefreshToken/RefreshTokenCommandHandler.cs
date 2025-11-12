using MediatR;
using NewsNet.Application.Abstractions.Authentication;
using NewsNet.Application.Contracts.Auth;

namespace NewsNet.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResponseDto?>
{
    private readonly IAuthenticationService _authenticationService;

    public RefreshTokenCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<AuthenticationResponseDto?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RefreshTokenAsync(
            request.RefreshToken,
            request.IpAddress,
            cancellationToken
        );

        if (result is null)
            return null;

        return new AuthenticationResponseDto(
            result.AccessToken,
            result.AccessTokenExpiresAt,
            result.RefreshToken,
            result.RefreshTokenExpiresAt,
            result.UserId,
            result.UserName,
            result.Email,
            result.Roles
        );
    }
}
