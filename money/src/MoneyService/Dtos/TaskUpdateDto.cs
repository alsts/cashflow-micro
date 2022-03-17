using System.ComponentModel.DataAnnotations;

namespace MoneyService.Dtos
{
    public class TaskUpdateDto
    {
        [Required] [MaxLength(50)] public string Title { get; set; }
        [Required] [MaxLength(50)] public string Description { get; set; }
    }
}
