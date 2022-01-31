using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Data.Models;

namespace TaskService.Services.interfaces
{
    public interface IUserService
    {
        Task<User> GetCurrent();
        Task<User> GetByPublicId(string id);
        Task<IEnumerable<User>> GetAll();
    }
}
