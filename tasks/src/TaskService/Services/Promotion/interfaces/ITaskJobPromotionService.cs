using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Data.Models;

namespace TaskService.Services.Promotion.interfaces
{
    public interface ITaskJobPromotionService
    {
        Task<User> GetCurrent();
        Task<User> GetByPublicId(string id);
        Task<IEnumerable<User>> GetAll();
    }
}
