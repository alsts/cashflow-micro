
using System.Collections.Generic;
using System.Threading.Tasks;
using ModerationService.Data.Models;

namespace ModerationService.Services.interfaces
{
    public interface IUserModerationService
    {
        Task<User> BanUser(string userId);
        Task<IEnumerable<User>> GetUsersToModerate();
    }
}
