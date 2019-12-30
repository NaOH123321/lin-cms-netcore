using LinCms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinCms.Infrastructure.Database.EntityConfigurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("book");

            builder.Property(e => e.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Author)
                .HasMaxLength(30)
                .HasDefaultValue("未名");

            builder.Property(e => e.Summary)
                .HasMaxLength(1000);

            builder.Property(e => e.Image)
                .HasMaxLength(50);
        }
    }
}
