using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = MoneyService.Data.Models.External.Task;

namespace MoneyService.Data.Repos.Interfaces
{
    public interface ITaskRepo
    {
        Task<TaskEntity> GetByPublicId(string publicId);
        Task Save(TaskEntity task);
        Task<IEnumerable<TaskEntity>> GetAll();
    }
}
