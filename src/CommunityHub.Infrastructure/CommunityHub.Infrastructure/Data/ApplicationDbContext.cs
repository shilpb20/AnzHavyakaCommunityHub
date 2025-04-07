using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Models.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Linq;

namespace CommunityHub.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<SpouseInfo> SpouseInfo { get; set; }
        public DbSet<Child> Children { get; set; }

        public DbSet<ContactForm> ContactForms { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyUniqueConstraintsToUserInfoTable(modelBuilder);
            ApplyUniqueConstraintsToSpouseInfoTable(modelBuilder);
            ApplyRelationships(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ApplyRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpouseInfo>()
                        .HasOne(r => r.UserInfo)
                        .WithOne(u => u.SpouseInfo)
                        .HasForeignKey<SpouseInfo>(r => r.UserInfoId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Child>()
                .HasOne(c => c.UserInfo)
                .WithMany(r => r.Children)
                .HasForeignKey(c => c.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInfo>()
               .HasOne(u => u.ApplicationUser)
               .WithOne()
               .HasForeignKey<UserInfo>(u => u.ApplicationUserId);
        }

        private void ApplyUniqueConstraintsToSpouseInfoTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpouseInfo>()
            .HasIndex(s => s.Email)
            .IsUnique();

            modelBuilder.Entity<SpouseInfo>()
                .HasIndex(s => new { s.CountryCode, s.ContactNumber })
                .IsUnique();
        }

        private void ApplyUniqueConstraintsToUserInfoTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
           .HasIndex(u => u.Email)
           .IsUnique();

            modelBuilder.Entity<UserInfo>()
                .HasIndex(s => new { s.CountryCode, s.ContactNumber })
                .IsUnique();
        }
    }
}