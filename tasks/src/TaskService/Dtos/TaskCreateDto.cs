using System.ComponentModel.DataAnnotations;
using GenericEventDto = Cashflow.Common.Dtos;

namespace TaskService.Dtos
{
    public class TaskCreateDto : GenericEventDto
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
    }
}
