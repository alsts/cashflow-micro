using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = TaskService.Data.Models.Task;
using TaskStatus = Cashflow.Common.Data.Enums.TaskStatus;

namespace TaskService.Data.Repos.Interfaces
{
    public interface ITaskRepo
    {
        Task<TaskEntity> GetByPublicId(string publicId);
        Task Save(TaskEntity task);
        Task<IEnumerable<TaskEntity>> GetAll();
        Task<IEnumerable<TaskEntity>> GetByUserId(int userId);
        Task<List<TaskEntity>> GetTasksByStatus(TaskStatus taskStatus);
    }
}
