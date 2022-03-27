using System.Threading.Tasks;
using MoneyService.Data.Models;
using MoneyService.Data.Models.External;

namespace MoneyService.Services.interfaces
{
    public interface IUsersService
    {
        Task<User> GetCurrentUser();
    }
}
