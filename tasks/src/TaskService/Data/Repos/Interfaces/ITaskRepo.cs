using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Data.Repos.Interfaces
{
    public interface ITaskRepo
    {
        Task<TaskEntity> GetById(int id);
        Task<TaskEntity> GetByPublicId(string publicId);
        Task Save(TaskEntity task);
        Task<IEnumerable<TaskEntity>> GetAll();
        Task<IEnumerable<TaskEntity>> GetByUserId(int userId);
    }
}
