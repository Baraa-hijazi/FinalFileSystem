using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyFileSystem.Core.Entities;
using MyFileSystem.Entities;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;

namespace MyFileSystem.Persistence
{
    public class FileSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Entities.File> Files { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public FileSystemDbContext() { } 

        //protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        public FileSystemDbContext(DbContextOptions<FileSystemDbContext> Options) : base(Options)
        { 
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build(); 
        }

        public class Fix : IDesignTimeDbContextFactory<FileSystemDbContext>
        {
            public FileSystemDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<FileSystemDbContext>();
                optionsBuilder.UseSqlServer("Server=localhost;Database=MyFileSystemDatabase;Trusted_Connection=True;");
                return new FileSystemDbContext(optionsBuilder.Options);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            var hash = new PasswordHasher<ApplicationUser>();
            var ADMIN_ID = Guid.NewGuid().ToString();
            var ROLE_ID = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            { Id = ROLE_ID, Name = "SuperAdmin", NormalizedName = "SuperAdmin".ToUpper() });

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin".ToUpper(),
                Email = "developer@gmail.com",
                NormalizedEmail = "developer@gmail.com".ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+962788000000000",
                PhoneNumberConfirmed = true,
                PasswordHash = hash.HashPassword(null, "P@ssw0rd"),
                SecurityStamp = String.Empty,
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
        }
    }
}
