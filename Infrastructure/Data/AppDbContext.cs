using Domain.Entities;
using Domain.Entities.TaskModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Domain.Entities.TaskModels.Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed roles
            var user = new IdentityRole<int> { Id = 1, Name = "user", NormalizedName = "USER" };
            builder.Entity<IdentityRole<int>>().HasData(user);

            var admin = new IdentityRole<int> { Id = 2, Name = "admin", NormalizedName = "ADMIN" };
            builder.Entity<IdentityRole<int>>().HasData(admin);
        }
    }
}
