using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccountService.Data.Models;
using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateVersionedEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateVersionedEntities();
            return base.SaveChanges();
        }

        private void UpdateVersionedEntities()
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
            }
        }
    }
}
