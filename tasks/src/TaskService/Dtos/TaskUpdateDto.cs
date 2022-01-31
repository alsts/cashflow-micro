using System.ComponentModel.DataAnnotations;

namespace TaskService.Dtos
{
    public class TaskUpdateDto
    {
        [Required] [MaxLength(50)] public string Title { get; set; }
        [Required] [MaxLength(50)] public string Description { get; set; }
    }
}
