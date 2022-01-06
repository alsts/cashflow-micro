using System.ComponentModel.DataAnnotations;

namespace AccountService.Dtos
{
    public class UserSignUpDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public int Gender { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
    }
}
