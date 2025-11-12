using MediatR;
using NewsNet.Application.Contracts.Auth;

namespace NewsNet.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password,
    string? IpAddress = null) : IRequest<AuthenticationResponseDto?>;
