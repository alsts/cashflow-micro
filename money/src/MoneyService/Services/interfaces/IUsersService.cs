using System.Threading.Tasks;
using MoneyService.Data.Models;

namespace MoneyService.Services.interfaces
{
    public interface IUsersService
    {
        Task<User> GetCurrentUser();
    }
}
