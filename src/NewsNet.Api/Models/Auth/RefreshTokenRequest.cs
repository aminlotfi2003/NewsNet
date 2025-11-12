using System.ComponentModel.DataAnnotations;

namespace NewsNet.Api.Models.Auth;

public sealed class RefreshTokenRequest
{
    [Required]
    [MaxLength(1024)]
    public string RefreshToken { get; set; } = string.Empty;
}
