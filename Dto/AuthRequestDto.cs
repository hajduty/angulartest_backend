using System.ComponentModel.DataAnnotations;

namespace backend.Dto;

public class AuthRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}