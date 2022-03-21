using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = ModerationService.Data.Models.External.Task;

namespace ModerationService.Data.Repos.Interfaces
{
    public interface ITaskRepo
    {
        Task<TaskEntity> GetByPublicId(string publicId);
        Task Save(TaskEntity task);
        Task<IEnumerable<TaskEntity>> GetAll();
        Task<IEnumerable<TaskEntity>> GetTasksPendingApproval();
    }
}
