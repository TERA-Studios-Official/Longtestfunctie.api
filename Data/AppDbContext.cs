using Longfunctie.api.Models;
using Microsoft.EntityFrameworkCore;

namespace Longfunctie.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Child>().ToTable("children"); // Explicit MySQL table
            modelBuilder.Entity<Child>().HasKey(c => c.Id);   // PK
            modelBuilder.Entity<Child>()
                        .Property(c => c.ParentName)
                        .IsRequired()
                        .HasMaxLength(20);                 // Matches MySQL schema
        }
    }
}
