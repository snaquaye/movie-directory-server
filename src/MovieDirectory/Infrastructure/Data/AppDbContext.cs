using Microsoft.EntityFrameworkCore;

using MovieDirectory.Core.Entities;

using Namotion.Reflection;

namespace MovieDirectory.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<SearchHistory> SearchHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SearchHistory>().HasKey(e => e.Id);
            builder.Entity<SearchHistory>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<SearchHistory>().Property(e => e.CreatedOn).HasDefaultValueSql("DATETIME('now')");
        }
    }
}
