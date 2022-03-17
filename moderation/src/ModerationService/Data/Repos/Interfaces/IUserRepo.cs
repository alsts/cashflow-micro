using System.Collections.Generic;
using System.Threading.Tasks;
using ModerationService.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace ModerationService.Data.Repos.Interfaces
{
    public interface IUserRepo
    {
        Task<User> GetByPublicId(string publicId);
        Task<User> GetUserByPublicIdAndRefreshToken(string publicId, string refreshToken);
        Task<IEnumerable<User>> GetAll();
        Task Save(User user);
        Task<IEnumerable<User>> GetUsersToModerate();
    }
}
