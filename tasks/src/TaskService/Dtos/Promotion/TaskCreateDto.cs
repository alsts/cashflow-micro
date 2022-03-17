using System.ComponentModel.DataAnnotations;

namespace TaskService.Dtos.Promotion
{
    public class TaskCreateDto 
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
    }
}
