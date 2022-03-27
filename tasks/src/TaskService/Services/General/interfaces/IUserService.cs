using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Data.Models;
using TaskService.Data.Models.External;

namespace TaskService.Services.General.interfaces
{
    public interface IUserService
    {
        Task<User> GetCurrent();
        Task<User> GetByPublicId(string id);
        Task<IEnumerable<User>> GetAll();
    }
}
