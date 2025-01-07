namespace Falcon.XorYDatabase
{
    using System.IO;
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Falcon.XorYDatabase.Models.Sql;

    public class XorYDBContext : DbContext
    {
        public XorYDBContext()
        {
        }

        public XorYDBContext(DbContextOptions<XorYDBContext> options)
            : base(options)
        {
        }

        public DbSet<XorYOption> Options { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", false)
                    .Build();

                var connectionString = configuration.GetConnectionString("XorYDBContext");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<XorYOption>()
                .HasKey(ct => ct.ID);
        }
    }
}
