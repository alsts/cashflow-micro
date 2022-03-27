using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Dtos.Promotion;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Services.Promotion.interfaces
{
    public interface ITaskPromotionService
    {
        Task<TaskEntity> Create(TaskCreateDto taskCreateDto);
        Task<TaskEntity> Update(TaskUpdateDto model, string publicId);
        Task<TaskEntity> GetByPublicId(string id);
        Task<IEnumerable<TaskEntity>> GetAll();
        Task<IEnumerable<TaskEntity>> GetForCurrentUser();
        Task StartTask(string publicId);
        Task StopTask(string publicId);
    }
}
