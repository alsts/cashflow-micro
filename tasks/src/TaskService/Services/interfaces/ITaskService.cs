using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Dtos;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Services.interfaces
{
    public interface ITaskService
    {
        Task<TaskEntity> Create(TaskCreateDto taskCreateDto);
        Task<TaskEntity> Update(TaskUpdateDto model, string publicId);
        Task<TaskEntity> GetByPublicId(string id);
        Task<IEnumerable<TaskEntity>> GetAll();
    }
}
