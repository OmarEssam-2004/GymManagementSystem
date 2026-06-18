using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagementSystem.DbContexts
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext>  Options) : base(Options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Plan> Plans { get; set; }

    }

}
