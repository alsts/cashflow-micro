using System.ComponentModel.DataAnnotations;

namespace AccountService.Data.Models
{
    public class Role
    {
        [Key] [Required] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
    }
}
