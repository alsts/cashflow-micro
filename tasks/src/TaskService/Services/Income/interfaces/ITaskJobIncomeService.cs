using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Data.Models;
using TaskService.Dtos.Income;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Services.Income.interfaces
{
    public interface ITaskJobIncomeService
    {
        Task<IEnumerable<TaskJob>> GetForCurrentUser();
        Task<TaskJob> GetById(string jobId);
        Task SubmitReport(string jobId, TaskJobReportDto report);
        Task SubmitReportImprovement(string jobId, TaskJobReportImprovementDto reportImprovement);
        Task Cancel(string jobId);
        Task<bool> CanUserStartWorkingOnTask(string taskId);
        Task<object> StartWorkingOnTask(string taskId);
    }
}
