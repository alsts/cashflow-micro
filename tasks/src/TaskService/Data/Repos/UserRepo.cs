using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskService.Data.Models;
using TaskService.Data.Models.External;
using TaskService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Data.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext context;

        public UserRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<User> GetById(int id)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<User> GetUserByPublicIdAndRefreshToken(string publicId, string refreshToken)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.PublicId == publicId && p.RefreshToken == refreshToken);
        }
        
        public async Task<User> GetByPublicId(string publicId)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.PublicId == publicId);
        }
        
        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.ToListAsync();
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
        
        private async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
