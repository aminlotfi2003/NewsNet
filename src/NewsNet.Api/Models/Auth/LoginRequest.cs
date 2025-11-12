using System.ComponentModel.DataAnnotations;

namespace NewsNet.Api.Models.Auth;

public sealed class LoginRequest
{
    [Required]
    [MaxLength(256)]
    public string UserNameOrEmail { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
