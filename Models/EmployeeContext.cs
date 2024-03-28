using Microsoft.EntityFrameworkCore;
using DatatableCRUD.Models;

namespace DatatableCRUD.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsTag>().HasKey(nt => new { nt.NewsId, nt.TagId });
          
        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

    }
}
