using CodeServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CodeServer.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<sdlc_system> sdlc_system { get; set; }
        public DbSet<project> project { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<project>()
                .HasIndex(p => new { p.external_id, p.sdlc_systemid })
                .IsUnique(true);
        }
    }
    //public class AppDbContextMigrationFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    //{
    //    public static readonly IConfigurationRoot ConfigBuilder = new ConfigurationBuilder()
    //             .SetBasePath(AppContext.BaseDirectory)
    //             .AddJsonFile("appsettings.json", false, true).Build();

    //    public ApplicationDbContext CreateDbContext(string[] args)
    //    {
    //        return new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
    //                               .UseSqlServer(ConfigBuilder.GetConnectionString("DefaultConnection"))
    //                               .Options);//DefaultConnection
    //    }
    //}
}
