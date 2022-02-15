using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Utils;
using Microsoft.EntityFrameworkCore;
using TaskService.Data.Models;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Data
{
    public class AppDbContext : DbContext
    {
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public AppDbContext(DbContextOptions<AppDbContext> opt, LoggedInUserDataHolder loggedInUserDataHolder) : base(opt)
        {
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasMany(p => p.Tasks)
                .WithOne(p => p.User!)
                .HasForeignKey(p => p.UserId);

            modelBuilder
                .Entity<TaskEntity>()
                .HasOne(p => p.User)
                .WithMany(p => p.Tasks)
                .HasForeignKey(p => p.UserId);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            DatabaseUtils.ConfigureAddedEntitiesChanges(ChangeTracker, loggedInUserDataHolder);
            DatabaseUtils.ConfigureModifiedEntitiesChanges(ChangeTracker, loggedInUserDataHolder);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            DatabaseUtils.ConfigureAddedEntitiesChanges(ChangeTracker, loggedInUserDataHolder);
            DatabaseUtils.ConfigureModifiedEntitiesChanges(ChangeTracker, loggedInUserDataHolder);
            return base.SaveChanges();
        }
    }
}
