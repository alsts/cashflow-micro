using System.Collections.Generic;
using System.Threading.Tasks;
using AccountService.Data.Models;
using AccountService.Data.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext context;

        public UserRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<User> GetByPublicId(string publicId)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.PublicId == publicId);
        }

        public async Task Save(User user)
        {
            if (user.Id != 0)
            {
                context.Users.Update(user);
            }
            else
            {
                await context.Users.AddAsync(user);
            }
            
            await SaveChanges();
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string hashedPassword)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.UserName == username && p.Password == hashedPassword);
        }

        public async Task<User> GetUserByPublicIdAndRefreshToken(string publicId, string refreshToken)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.PublicId == publicId && p.RefreshToken == refreshToken);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.UserName == username);
        }

        private async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
