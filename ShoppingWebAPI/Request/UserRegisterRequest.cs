using System.ComponentModel.DataAnnotations;

namespace ShoppingWebAPI.Request;

public class UserRegisterRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}