using MediatR;
using NewsNet.Application.Abstractions.Authentication;
using NewsNet.Application.Contracts.Auth;

namespace NewsNet.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponseDto?>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<AuthenticationResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.SignInAsync(
            request.UserNameOrEmail,
            request.Password,
            request.IpAddress,
            cancellationToken);

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
            result.Roles);
    }
}
