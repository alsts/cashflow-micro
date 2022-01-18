using System.ComponentModel.DataAnnotations;

namespace AccountService.Dtos
{
    public class UserUpdateDto
    {
        [Required] [MaxLength(50)] public string Email { get; set; }
        [Required] [MaxLength(50)] public string Firstname { get; set; }
        [Required] [MaxLength(50)] public string Lastname { get; set; }
    }
}
