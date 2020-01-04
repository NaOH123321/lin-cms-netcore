using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinCms.Infrastructure.Database.EntityConfigurations
{
    public class LinUserConfiguration : IEntityTypeConfiguration<LinUser>
    {
        public void Configure(EntityTypeBuilder<LinUser> builder)
        {
            builder.ToTable("lin_user");

            builder.Property(e => e.Username)
                .HasComment("用户名")
                .HasMaxLength(24)
                .IsRequired();

            builder.Property(e => e.Nickname)
                .HasComment("昵称")
                .HasMaxLength(24);

            builder.Property(e => e.Avatar)
                .HasComment("头像url")
                .HasMaxLength(255);

            builder.Property(e => e.Admin)
                .HasComment("是否为超级管理员 ;  1 -> 普通用户 |  2 -> 超级管理员")
                .HasDefaultValue((short)1)
                .IsRequired();

            builder.Property(e => e.Active)
                .HasComment("当前用户是否为激活状态，非激活状态默认失去用户权限 ; 1 -> 激活 | 2 -> 非激活")
                .HasDefaultValue((short)1)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasComment("电子邮箱")
                .HasMaxLength(100);

            builder.Property(e => e.GroupId)
                .HasComment("用户所属的权限组id");

            builder.Property(e => e.Password)
                .HasComment("密码")
                .HasMaxLength(100);

            builder.HasIndex(p => p.Username)
                .HasName("uk_username")
                .IsUnique();

            builder.HasIndex(p => p.Nickname)
                .HasName("uk_nickname")
                .IsUnique();

            builder.HasIndex(p => p.Email)
                .HasName("uk_email")
                .IsUnique();

            builder.HasIndex(p => p.GroupId)
                .HasName("idx_group_id");

            builder.HasOne(b => b.LinGroup)
                .WithMany(o => o.LinUsers)
                .HasPrincipalKey(o => o.Id)
                .HasForeignKey(b => b.GroupId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
