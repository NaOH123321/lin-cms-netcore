using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinCms.Infrastructure.Database.EntityConfigurations
{
    public class LinFileConfiguration : IEntityTypeConfiguration<LinFile>
    {
        public void Configure(EntityTypeBuilder<LinFile> builder)
        {
            builder.ToTable("lin_file");

            builder.Property(e => e.Path)
                .HasComment("路径")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(e => e.Type)
                .HasComment("1 local，其他表示其他地方")
                .HasDefaultValue((short)1);

            builder.Property(e => e.Name)
                .HasComment("名称")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Extension)
                .HasComment("后缀")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Size)
                .HasComment("大小");

            builder.Property(e => e.Md5)
                .HasComment("图片md5值，防止上传重复图片")
                .HasMaxLength(40);

            builder.HasIndex(p => p.Md5)
                .HasName("uk_md5")
                .IsUnique();
        }
    }
}
