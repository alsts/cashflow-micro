using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModerationService.Data.Models;
using ModerationService.Data.Models.External;
using ModerationService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace ModerationService.Data.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext context;
        private const int WARNING_COUNT_THRESHOLD = 5;

        public UserRepo(AppDbContext context)
        {
            this.context = context;
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
        
        public async Task Ban(UserBan userBan)
        {
            if (userBan.Id != 0)
            {
                context.UserBans.Update(userBan);
            }
            else
            {
                await context.UserBans.AddAsync(userBan);
            }

            await SaveChanges();
        }

        public async Task<IEnumerable<User>> GetUsersToModerate()
        {
            var userBans = await context.UserBans.ToListAsync();
            var bannedUsersIds = userBans.Select(x => x.UserId);

            return await context.Users.Where(x =>
                    !x.IsBanned
                    && !bannedUsersIds.Contains(x.PublicId)
                    // && x.WarningsCount > WARNING_COUNT_THRESHOLD
            ).ToListAsync();
        }

        private async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
