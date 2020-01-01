using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinCms.Infrastructure.Database.EntityConfigurations
{
    public class LinAuthConfiguration : IEntityTypeConfiguration<LinAuth>
    {
        public void Configure(EntityTypeBuilder<LinAuth> builder)
        {
            builder.ToTable("lin_auth");

            builder.Property(e => e.GroupId)
                .HasComment("所属权限组id")
                .IsRequired();

            builder.Property(e => e.Auth)
                .HasComment("权限字段")
                .HasMaxLength(60);

            builder.Property(e => e.Module)
                .HasComment("权限的模块")
                .HasMaxLength(50);

            builder.HasIndex(p => p.GroupId)
                .HasName("idx_group_id");

            builder.HasOne(b => b.LinGroup)
                .WithMany(o => o.LinAuths)
                .HasPrincipalKey(o => o.Id)
                .HasForeignKey(b => b.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
