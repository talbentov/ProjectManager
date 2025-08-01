using Microsoft.EntityFrameworkCore;
using ProjectManager.Server.Models;

namespace ProjectManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId); // Primary key
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId); // Primary key
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasOne(e => e.User) // Each Project belongs to one User
                      .WithMany(e => e.Projects) // A User can have many Projects
                      .HasForeignKey(e => e.UserId) // Foreign key to User
                      .OnDelete(DeleteBehavior.Cascade); // If user deleted, all its projects deleted 
            });

            // Configure Task entity
            modelBuilder.Entity<ProjectTask>(entity =>
            {
                entity.HasKey(e => e.TaskId); // Primary key
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Project)
                      .WithMany(e => e.Tasks)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
