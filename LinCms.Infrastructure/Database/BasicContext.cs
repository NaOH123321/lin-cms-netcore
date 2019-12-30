using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LinCms.Infrastructure.Database
{
    public class BasicContext : DbContext
    {
        public BasicContext(DbContextOptions<LinContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            EntityConfiguration(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (!entry.Entity.GetType().IsSubclassOf(typeof(Entity))) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[nameof(Entity.CreateTime)] = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.CurrentValues[nameof(Entity.UpdateTime)] = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues[nameof(Entity.DeleteTime)] = DateTime.Now;
                        break;
                }
            }
        }

        private void EntityConfiguration(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IEntityInt).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasKey(nameof(IEntityInt.Id));

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntityInt.Id))
                        .ValueGeneratedOnAdd();
                }

                if (entityType.ClrType.IsSubclassOf(typeof(Entity)))
                {
                    var parameterExpression = Expression.Parameter(entityType.ClrType, "x");
                    var propertyExpression = Expression.Property(parameterExpression, nameof(Entity.DeleteTime));
                    var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(null));
                    var lambdaExpression = Expression.Lambda(equalExpression, parameterExpression);

                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(lambdaExpression);
                }

                // 用SnakeCase方式改写列名          
                foreach (var property in entityType.GetProperties())
                {
                    property.SetColumnName(property.Name.ToSnakeCase());
                }
            }
        }
    }
}
