using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
