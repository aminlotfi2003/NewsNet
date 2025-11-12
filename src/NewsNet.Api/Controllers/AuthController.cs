using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsNet.Api.Models.Auth;
using NewsNet.Application.Contracts.Auth;
using NewsNet.Application.Features.Auth.Commands.Login;
using NewsNet.Application.Features.Auth.Commands.RefreshToken;

namespace NewsNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthenticationResponseDto>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new LoginCommand(request.UserNameOrEmail, request.Password, GetIpAddress());
        var result = await _mediator.Send(command, cancellationToken);

        if (result is null)
            return Unauthorized();

        return Ok(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthenticationResponseDto>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new RefreshTokenCommand(request.RefreshToken, GetIpAddress());
        var result = await _mediator.Send(command, cancellationToken);

        if (result is null)
            return Unauthorized();

        return Ok(result);
    }

    private string? GetIpAddress()
        => HttpContext.Connection.RemoteIpAddress?.ToString();
}
