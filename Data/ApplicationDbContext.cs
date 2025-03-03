using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rethus_backend.Models;
using rethus_backend.Utilities.Constants.User.UserConfiguration;

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
        public DbSet<UserDigitalSignature> UserDigitalSignature { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RELATION MODEL USERFORM
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
                .Entity<User>()
                .HasOne(u => u.UserForm) // Un usuario tiene un formulario
                .WithOne(uf => uf.User) // Un formulario pertenece a un solo usuario
                .HasForeignKey<UserForm>(uf => uf.UserId) // UserId es la clave foránea en UserForm
                .OnDelete(DeleteBehavior.Cascade); // Opcional: comportamiento de eliminación

            modelBuilder
                .Entity<User>()
                .HasOne(u => u.Configurations) // Un usuario tiene un formulario
                .WithOne(uf => uf.User) // Un formulario pertenece a un solo usuario
                .HasForeignKey<Configurations>(uf => uf.UserId) // UserId es la clave foránea en UserForm
                .OnDelete(DeleteBehavior.Cascade); // Opcional: comportamiento de eliminación

            modelBuilder
                .Entity<User>()
                .HasOne(u => u.DigitalSignature) // Un usuario tiene una firma
                .WithOne(uf => uf.User) // Una firma pertenece a un solo usuario
                .HasForeignKey<UserDigitalSignature>(uf => uf.UserId) // UserId es la clave foránea en UserDigitalSignature
                .OnDelete(DeleteBehavior.Cascade); // Opcional: comportamiento de eliminación

            // INDICE MODEL USERFORM
            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => u.UserFormId) // Índice en CreatedAt
                .HasDatabaseName("IX_UserForm_UserFormId");

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => u.CreatedAt)
                .IsDescending()
                .HasDatabaseName("IX_UserForm_CreatedAt");

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => new { u.Status, u.CreatedAt })
                .HasDatabaseName("IX_UserForm_Status_CreatedAt");

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(
                    u =>
                        new
                        {
                            u.Status,
                            u.StepForm,
                            u.CreatedAt
                        }
                )
                .IsDescending()
                .HasDatabaseName("IX_UserForm_status_stepForm_createdAt_Include_properties")
                .IncludeProperties(
                    u =>
                        new
                        {
                            u.UserFormId,
                            u.PersonalFirstName,
                            u.PersonalIdentification,
                            u.TypeProcedure,
                            u.Consecutive,
                            u.UpdatedAt
                        }
                );

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => new { u.UserId, u.Status })
                .HasDatabaseName("IX_UserForm_UserId_Status");

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => new { u.PersonalTypeIdentification, u.PersonalIdentification })
                .HasDatabaseName("IX_UserForm_PersonalTypeIdentification_PersonalIdentification");

            modelBuilder
                .Entity<UserForm>()
                .HasIndex(u => new { u.Consecutive })
                .HasDatabaseName("IX_UserForm_Consecutive");

            // INDICE MODEL COMMENTS
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

            // INDICE MODEL USERFORMFILES
            modelBuilder
                .Entity<UserFormFiles>()
                .HasIndex(c => new { c.UserFormFilesId, })
                .HasDatabaseName("IX_UserForm_UserFormFilesId");

            modelBuilder
                .Entity<UserFormFiles>()
                .HasIndex(c => new { c.UserFormId, })
                .HasDatabaseName("IX_UserForm_UserFormId");

            // INDICE MODEL COMMENTS
            modelBuilder
                .Entity<Comments>()
                .HasIndex(c => new { c.CommentId, })
                .HasDatabaseName("IX_Comments_CommentId");

            // INDICE MODEL USERCONFIGURATION
            modelBuilder
                .Entity<Configurations>()
                .HasIndex(c => new { c.UserId, c.Step })
                .HasDatabaseName("IX_Configurations_UserId_Step");

            modelBuilder
                .Entity<Configurations>()
                .HasIndex(c => new { c.ConfigurationsId })
                .HasDatabaseName("IX_Configurations_UserId");

            // INDICE MODEL USER
            modelBuilder
                .Entity<User>()
                .HasIndex(c => new { c.UserId })
                .HasDatabaseName("IX_User_UserId");

            modelBuilder
                .Entity<User>()
                .HasIndex(c => new { c.email, c.Status })
                .HasDatabaseName("IX_User_Email_Status");

            modelBuilder
                .Entity<User>()
                .HasIndex(c => new { c.Token })
                .HasDatabaseName("IX_User_Token");

            // INDICE MODEL COUNTRY
            modelBuilder
                .Entity<Country>()
                .HasIndex(c => new { c.CountryId })
                .HasDatabaseName("IX_Country_CountryId");

            // INDICE MODEL DEPARMENTS
            modelBuilder
                .Entity<Department>()
                .HasIndex(c => new { c.CountryId })
                .HasDatabaseName("IX_Department_CountryId");

            // INDICE MODEL CITY
            modelBuilder
                .Entity<City>()
                .HasIndex(c => new { c.DepartmentId })
                .HasDatabaseName("IX_Department_DepartmentId");
        }
    }
}
