using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerWithDocker.Configuration;
using TaskManagerWithDocker.Core.Entities;
using TaskManagerWithDocker.Models.Entities;

namespace TaskManagerWithDocker.Data
{
    public class TaskManagerDbContext : IdentityDbContext<ApiUser>
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
        {
            
        }        

        // Registro de tabelas
        public DbSet<TaskManager> Tasks { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskManager>()
                .Property(t => t.Id)                
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
