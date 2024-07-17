using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rethus_backend.Models;

namespace rethus_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Configurations> Configurations { get; set; }
        public DbSet<UserForm> UserForm { get; set; }
        public DbSet<UserFormFiles> UserFormFiles { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<City> City { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Country>()
                .HasMany(c => c.Departments)
                .WithOne(d => d.Country)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Department>()
                .HasMany(d => d.City)
                .WithOne(c => c.Department)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<City>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
