using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Controllers;
using TaskService.Dtos;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Services.Income.interfaces
{
    public interface ITaskIncomeService
    {
        Task<TaskEntity> Create(TaskCreateDto taskCreateDto);
        Task<TaskEntity> Update(TaskUpdateDto model, string publicId);
        Task<TaskEntity> GetByPublicId(string id);
        Task<IEnumerable<TaskEntity>> GetAll();
        Task<IEnumerable<TaskEntity>> GetForCurrentUser();
        Task StartTask(string publicId);
        Task StopTask(string publicId);
        Task<IEnumerable<TaskEntity>> GetAvailableTasks();
        Task GetJobsByQuery(TasksQuery tasksQuery);
    }
}
