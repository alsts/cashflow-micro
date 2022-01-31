using System.Collections.Generic;
using System.Threading.Tasks;
using AccountService.Models;

namespace AccountService.Data
{
    public interface IUserRepo
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByPublicId(string publicId);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string username);
        Task Save(User user);
        Task<User> GetUserByUsernameAndPassword(string username, string hashedPassword);
        Task<User> GetUserByPublicIdAndRefreshToken(string publicId, string refreshToken);
    }
}
