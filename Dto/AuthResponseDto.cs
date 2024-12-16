using System.ComponentModel.DataAnnotations;

namespace backend.Dto;

public class AuthResponseDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Token { get; set; }
}
