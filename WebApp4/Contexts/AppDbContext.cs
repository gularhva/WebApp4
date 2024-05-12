using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp4.Configurations;
using WebApp4.Entities;
using WebApp4.Entities.Identities;

namespace WebApp4.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentConfig).Assembly);
            modelBuilder.Entity<Student>()
               .HasOne(s => s.School) // Use HasOne instead of HasRequired
               .WithMany(g => g.Students)
               .HasForeignKey(s => s.School_Id).OnDelete(DeleteBehavior.Cascade); // Use IsRequired method to specify the foreign key is required
        }
    }
}
