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
        public DbSet<ConfigurationSetting> ConfigurationSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder
                .Entity<UserForm>()
                .HasOne(uf => uf.CountryOfBirth)
                .WithMany()
                .HasForeignKey(uf => uf.PersonalCountryBirthId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasOne(p => p.DepartmentBirth)
                .WithMany()
                .HasForeignKey(p => p.PersonalDepartmentBirthId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasOne(p => p.MunicipalityBirth)
                .WithMany()
                .HasForeignKey(p => p.PersonalMunicipalityBirthId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasOne(p => p.PlaceResidence)
                .WithMany()
                .HasForeignKey(p => p.PersonalPlaceResidenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasOne(p => p.DepartmentResidence)
                .WithMany()
                .HasForeignKey(p => p.PersonalDepartmentResidenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasOne(p => p.MunicipalityResidence)
                .WithMany()
                .HasForeignKey(p => p.PersonalMunicipalityResidenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasOne(p => p.CountryInstitution)
                .WithMany()
                .HasForeignKey(p => p.AcademicsCountryInstitution)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasOne(p => p.DepartmentInstitution)
                .WithMany()
                .HasForeignKey(p => p.AcademicsDepartmentInstitutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasOne(p => p.MunicipalityInstitution)
                .WithMany()
                .HasForeignKey(p => p.AcademicsMunicipalityInstitutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => u.CreatedAt) // Índice en CreatedAt
                .HasDatabaseName("IX_UserForm_CreatedAt");

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => new { u.Status, u.CreatedAt }) // Índice compuesto en Status y CreatedAt
                .HasDatabaseName("IX_UserForm_Status_CreatedAt");

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => new { u.Status, u.CreatedAt }) // Claves del índice
                .HasDatabaseName("IX_UserForm_Coverage")
                .IncludeProperties(
                    u =>
                        new
                        {
                            u.UserFormId,
                            u.PersonalFirstName,
                            u.PersonalIdentification,
                            u.TypeProcedure,
                            u.StepForm,
                            u.Consecutive // Columnas incluidas en el índice para cobertura
                        }
                );

            modelBuilder
                .Entity<Comments>()
                .HasIndex(
                    c =>
                        new
                        {
                            c.UserFormId,
                            c.Status,
                            c.Type
                        }
                )
                .HasDatabaseName("IX_Comment_UserFormId_Status_Type");
        }
    }
}
