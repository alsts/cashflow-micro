using System.ComponentModel.DataAnnotations;

namespace AccountService.Dtos
{
    public class UserSignInDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
