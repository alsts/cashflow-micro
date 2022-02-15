using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccountService.Data.Models;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Data.Models;
using Cashflow.Common.Utils;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data
{
    public class AppDbContext : DbContext
    {
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public AppDbContext(DbContextOptions<AppDbContext> opt, LoggedInUserDataHolder loggedInUserDataHolder) : base(opt)
        {
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ConfigureModifiedEntitiesChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            DatabaseUtils.ConfigureAddedEntitiesChanges(ChangeTracker, loggedInUserDataHolder);
            ConfigureModifiedEntitiesChanges();
            return base.SaveChanges();
        }

        private void ConfigureModifiedEntitiesChanges()
        {
            foreach (var entity in ChangeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity && 
                            x.State == EntityState.Modified && 
                            // dont increase version if refreshToken was modified for User
                            !(x.Entity is User && x.Property("RefreshToken").CurrentValue != x.Property("RefreshToken").OriginalValue))
                .Select(x => x.Entity)
                .Cast<BaseEntity>())
            {
                entity.Version += 1;
                entity.LastUpdatedAt = DateTime.Now;
                entity.LastUpdatedByUserId = loggedInUserDataHolder?.UserID ?? "";
            }
        }
    }
}
