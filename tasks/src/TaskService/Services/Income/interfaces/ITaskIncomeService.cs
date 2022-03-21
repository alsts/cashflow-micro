using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Services.Income.interfaces
{
    public interface ITaskIncomeService
    {
        Task<TaskEntity> GetByPublicId(string id);
        Task<IEnumerable<TaskEntity>> GetAvailableTasks();
    }
}
