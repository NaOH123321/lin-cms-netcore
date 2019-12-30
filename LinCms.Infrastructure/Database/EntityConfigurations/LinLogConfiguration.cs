using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinCms.Infrastructure.Database.EntityConfigurations
{
    public class LinLogConfiguration : IEntityTypeConfiguration<LinLog>
    {
        public void Configure(EntityTypeBuilder<LinLog> builder)
        {
            builder.ToTable("lin_log");

            builder.Property(e => e.Message)
                .HasComment("日志信息")
                .HasMaxLength(450);

            builder.Property(e => e.Time)
                .HasComment("日志创建时间");

            builder.Property(e => e.UserId)
                .HasComment("用户id")
                .IsRequired();

            builder.Property(e => e.UserName)
                .HasComment("用户当时的昵称")
                .HasMaxLength(20);

            builder.Property(e => e.StatusCode)
                .HasComment("请求的http返回码");

            builder.Property(e => e.Method)
                .HasComment("请求方法")
                .HasMaxLength(20);

            builder.Property(e => e.Path)
                .HasComment("请求路径")
                .HasMaxLength(50);

            builder.Property(e => e.Authority)
                .HasComment("访问哪个权限")
                .HasMaxLength(100);
        }
    }
}
