using Microsoft.EntityFrameworkCore;
using ProfileService.Models;

namespace ProfileService.Data
{
    public class ProfileServiceDBContext : DbContext
    {
        public ProfileServiceDBContext(DbContextOptions<ProfileServiceDBContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BasicInfo> BasicInfos { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<FamilyInfo> FamilyInfos { get; set; }
        public DbSet<Lifestyle> Lifestyles { get; set; }
        public DbSet<PhysicalAttributes> PhysicalAttributes { get; set; }
        public DbSet<Educations> Educations { get; set; }
        public DbSet<Careers> Careers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(up => up.Id);
                entity.Property(up => up.Id).ValueGeneratedOnAdd();
                entity.Property(up => up.UserId).IsRequired();
                entity.HasOne(up => up.BasicInfo)
                      .WithOne()
                      .HasForeignKey<UserProfile>(up => up.BasicInfoId);
                entity.HasOne(up => up.Address)
                      .WithOne()
                      .HasForeignKey<UserProfile>(up => up.AddressId)
                      .IsRequired(false);
                entity.HasOne(up => up.PhysicalAttribute)
                      .WithOne()
                      .HasForeignKey<UserProfile>(up => up.PhysicalAttrId)
                      .IsRequired(false);
                entity.HasOne(up => up.FamilyInfo)
                      .WithOne()
                      .HasForeignKey<UserProfile>(up => up.FamilyInfoId)
                      .IsRequired(false);
                entity.HasOne(up => up.LifeStyle)
                      .WithOne()
                      .HasForeignKey<UserProfile>(up => up.LifeStyleId)
                      .IsRequired(false);
                entity.HasMany(up => up.Educations)
                      .WithOne(e => e.UserProfile)
                      .HasForeignKey(e => e.UserProfileId);
                entity.HasMany(up => up.Careers)
                      .WithOne(c => c.UserProfile)
                      .HasForeignKey(c => c.UserProfileId);
            });

            modelBuilder.Entity<BasicInfo>(entity =>
            {
                entity.HasKey(bi => bi.Id);
                entity.Property(bi => bi.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<FamilyInfo>(entity =>
            {
                entity.HasKey(fi => fi.Id);
                entity.Property(fi => fi.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Lifestyle>(entity =>
            {
                entity.HasKey(ls => ls.Id);
                entity.Property(ls => ls.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PhysicalAttributes>(entity =>
            {
                entity.HasKey(pa => pa.Id);
                entity.Property(pa => pa.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Educations>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(e => e.UserProfile)
                      .WithMany(up => up.Educations)
                      .HasForeignKey(e => e.UserProfileId);
            });

            modelBuilder.Entity<Careers>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.HasOne(c => c.UserProfile)
                      .WithMany(up => up.Careers)
                      .HasForeignKey(c => c.UserProfileId);
            });
        }
    }
}
