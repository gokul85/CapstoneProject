using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AuthServiceDBContext : DbContext
    {
        public AuthServiceDBContext(DbContextOptions<AuthServiceDBContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Phone).IsRequired().HasMaxLength(15);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(50);

            });

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.HasKey(ud => ud.UserId);
                entity.Property(ud => ud.Password).IsRequired();
                entity.Property(ud => ud.PasswordHashKey).IsRequired();
                entity.Property(ud => ud.Status).IsRequired().HasMaxLength(50);
                entity.Property(ud => ud.IsPremium).IsRequired();

                entity.HasOne<User>()
                     .WithOne()
                     .HasForeignKey<UserDetails>(ud => ud.UserId);
            });
        }
    }
}
