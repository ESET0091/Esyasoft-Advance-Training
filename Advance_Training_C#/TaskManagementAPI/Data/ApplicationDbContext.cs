using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed initial data
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Learn ASP.NET Core",
                    Description = "Complete REST API tutorial",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    DueDate = DateTime.UtcNow.AddDays(7),
                    Priority = TaskPriority.High
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Build Task Management API",
                    Description = "Implement CRUD operations",
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    DueDate = DateTime.UtcNow.AddDays(14),
                    Priority = TaskPriority.Medium
                }
            );
        }
    }
}
