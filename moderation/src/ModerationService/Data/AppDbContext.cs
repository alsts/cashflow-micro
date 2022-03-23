using System.Threading;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Utils;
using Microsoft.EntityFrameworkCore;
using ModerationService.Data.Models;
using ModerationService.Data.Models.External;
using TaskEntity = ModerationService.Data.Models.External.Task;

namespace ModerationService.Data
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
            // enum conversion:
            modelBuilder.Entity<TaskEntity>()
                .Property(c => c.TaskStatus)
                .HasConversion<int>();
            
            modelBuilder.Entity<User>()
                .Property(c => c.Gender)
                .HasConversion<int>();
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
