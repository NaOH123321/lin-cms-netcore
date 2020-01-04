using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinCms.Infrastructure.Database.EntityConfigurations
{
    public class LinEventConfiguration : IEntityTypeConfiguration<LinEvent>
    {
        public void Configure(EntityTypeBuilder<LinEvent> builder)
        {
            builder.ToTable("lin_event");

            builder.Property(e => e.GroupId)
                .HasComment("所属权限组id")
                .IsRequired();

            builder.Property(e => e.MessageEvents)
                .HasComment("信息")
                .HasMaxLength(250);

            builder.HasIndex(p => p.GroupId)
                .HasName("idx_group_id");

            builder.HasOne(b => b.LinGroup)
                .WithMany(o => o.LinEvents)
                .HasPrincipalKey(o => o.Id)
                .HasForeignKey(b => b.GroupId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
