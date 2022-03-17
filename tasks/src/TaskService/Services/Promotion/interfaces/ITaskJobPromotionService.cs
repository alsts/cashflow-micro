using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Controllers.Promotion;
using TaskService.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Services.Promotion.interfaces
{
    public interface ITaskJobPromotionService
    {
        Task<TaskJob> GetById(string id);
        Task<IEnumerable<TaskJob>> GetAll();
        Task<IEnumerable<TaskJob>> GetJobsByQuery(TasksQuery tasksQuery);
        Task Approve(string jobId);
        Task Decline(string jobId);
        Task RequestImprovement(string jobId);
    }
}
