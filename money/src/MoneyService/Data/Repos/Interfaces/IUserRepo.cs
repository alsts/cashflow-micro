using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyService.Data.Models;
using MoneyService.Data.Models.External;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Data.Repos.Interfaces
{
    public interface IUserRepo
    {
        Task<User> GetByPublicId(string publicId);
        Task<User> GetUserByPublicIdAndRefreshToken(string publicId, string refreshToken);
        Task<IEnumerable<User>> GetAll();
        Task Save(User user);
    }
}
