using Microsoft.EntityFrameworkCore;
using GraphQLServer.Models;
using System.Reflection;

namespace GraphQLServer.Data
{
    public class BlogContext:DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options):base(options)
        {
        }
        public DbSet<Inventory> Inventories { get; set; } = default!;
        public DbSet<Items> Items { get; set; } = default!;
        public DbSet<MeasurementUnits> MeasurementUnits { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
