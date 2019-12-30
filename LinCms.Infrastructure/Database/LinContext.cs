using System;
using System.Collections.Generic;
using System.Linq;
using LinCms.Core.Entities;
using LinCms.Infrastructure.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace LinCms.Infrastructure.Database
{
    public class LinContext : BasicContext
    {
        public LinContext(DbContextOptions<LinContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LinFileConfiguration());
            modelBuilder.ApplyConfiguration(new LinEventConfiguration());
            modelBuilder.ApplyConfiguration(new LinGroupConfiguration());
            modelBuilder.ApplyConfiguration(new LinLogConfiguration());
            modelBuilder.ApplyConfiguration(new LinUserConfiguration());
            modelBuilder.ApplyConfiguration(new LinAuthConfiguration());

            modelBuilder.ApplyConfiguration(new BookConfiguration());
        }

        public DbSet<LinFile> LinFiles { get; set; } = null!;
        public DbSet<LinEvent> LinEvents { get; set; } = null!;
        public DbSet<LinLog> LinLogs { get; set; } = null!;
        public DbSet<LinGroup> LinGroups { get; set; } = null!;
        public DbSet<LinUser> LinUsers { get; set; } = null!;
        public DbSet<LinAuth> LinAuths { get; set; } = null!;

        public DbSet<Book> Books { get; set; } = null!;
    }
}
