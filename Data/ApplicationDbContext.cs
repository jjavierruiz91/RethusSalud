using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rethus_backend.Models;

namespace rethus_backend.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Configurations> Configurations { get; set; }
    public DbSet<UserForm> UserForm { get; set; }
    public DbSet<UserFormFiles> UserFormFiles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //   modelBuilder.Entity<User>().ToTable("User");
      //   modelBuilder.Entity<Configurations>().ToTable("Configurations");
      //   modelBuilder.Entity<UserForm>().ToTable("UserForm");
      //   modelBuilder.Entity<UserFormFiles>().ToTable("UserFormFiles");

      //   modelBuilder.Entity<Configurations>()
      //     .HasNoKey();

      //   modelBuilder.Entity<UserForm>()
      //     .HasNoKey();

      //   modelBuilder.Entity<UserFormFiles>()
      // .HasNoKey();

      //   modelBuilder.Entity<UserFormFiles>().HasOne(userForm => userForm.UserForm).WithOne(user => user.UserFormFiles);

      base.OnModelCreating(modelBuilder);
    }

  }
}