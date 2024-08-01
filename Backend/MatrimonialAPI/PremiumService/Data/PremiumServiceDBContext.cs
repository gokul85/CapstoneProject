using Microsoft.EntityFrameworkCore;
using PremiumService.Models;

namespace PremiumService.Data
{
    public class PremiumServiceDBContext : DbContext
    {
        public PremiumServiceDBContext(DbContextOptions<PremiumServiceDBContext> options) : base(options)
        {
            
        }

        public DbSet<Payments> Payments { get; set; }
        public DbSet<ContactViews> ContactViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ContactViews>(entity =>
            {
                entity.HasKey(cv => cv.Id);
                entity.Property(cv => cv.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
