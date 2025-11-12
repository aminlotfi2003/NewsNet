using MediatR;
using NewsNet.Application.Contracts.Auth;

namespace NewsNet.Application.Features.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken, string? IpAddress = null)
    : IRequest<AuthenticationResponseDto?>;
