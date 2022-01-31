using System.Collections.Generic;
using System.Threading.Tasks;
using TaskService.Data.Models;

namespace TaskService.Data.Repos.Interfaces
{
    public interface IUserRepo
    {
        Task<User> GetById(int id);
        Task<User> GetByPublicId(string publicId);
        Task<User> GetUserByPublicIdAndRefreshToken(string publicId, string refreshToken);
        Task<IEnumerable<User>> GetAll();
    }
}
