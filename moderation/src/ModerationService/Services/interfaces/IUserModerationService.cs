
using System.Collections.Generic;
using System.Threading.Tasks;
using ModerationService.Data.Models;
using ModerationService.Data.Models.External;

namespace ModerationService.Services.interfaces
{
    public interface IUserModerationService
    {
        Task<User> BanUser(string userId);
        Task<IEnumerable<User>> GetUsersToModerate();
    }
}
