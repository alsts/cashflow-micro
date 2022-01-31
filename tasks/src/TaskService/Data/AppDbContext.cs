using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskService.Data.Models;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {}

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
                .Where(x => x.Entity is BaseEntity && x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .Cast<BaseEntity>())
            {
                entity.Version += 1;
            }
        }
    }
}
