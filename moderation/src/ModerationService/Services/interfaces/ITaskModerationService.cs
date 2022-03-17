
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = ModerationService.Data.Models.Task;

namespace ModerationService.Services.interfaces
{
    public interface ITaskModerationService
    {
        Task<IEnumerable<TaskEntity>> GetTasksToModerate();
        Task<TaskEntity> ApproveTask(string taskId);
    }
}
