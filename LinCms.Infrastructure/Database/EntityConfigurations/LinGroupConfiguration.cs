using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinCms.Infrastructure.Database.EntityConfigurations
{
    public class LinGroupConfiguration : IEntityTypeConfiguration<LinGroup>
    {
        public void Configure(EntityTypeBuilder<LinGroup> builder)
        {
            builder.ToTable("lin_group");

            builder.Property(e => e.Name)
                .HasComment("权限组名称")
                .HasMaxLength(60);

            builder.Property(e => e.Info)
                .HasComment("权限组描述")
                .HasMaxLength(255);
        }
    }
}
