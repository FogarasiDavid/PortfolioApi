using Elasticsearch.Net;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities;
using Portfolio.Domain.Entity;
using System.Reflection;

namespace Portfolio.Infrastructure.Database
{
    public class PortfolioDbContext : IdentityDbContext<ApplicationUser>
    {
        //2 entity dbset
        public DbSet<Project> Project { get; set; }
        public DbSet<Technology> Technology { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //automatikussan alkalmazza minden osztályt ami megvalósítja
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortfolioDbContext).Assembly);
            //many to many kapcsolat
            modelBuilder.Entity<Project>()
                 .HasMany(p => p.Technologys)
                 .WithMany();
        }
    }
}