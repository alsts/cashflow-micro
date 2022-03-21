using System.Threading;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Utils;
using Microsoft.EntityFrameworkCore;
using MoneyService.Data.Models;
using TaskEntity = MoneyService.Data.Models.Task;

namespace MoneyService.Data
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
        public DbSet<UserTransaction> UserTransactions { get; set; }
        public DbSet<TaskTransaction> TaskTransactions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
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
