using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace DAL.Configuration
{
    public class TitleConfiguration : IEntityTypeConfiguration<TitleBook>
    {
        public void Configure(EntityTypeBuilder<TitleBook> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(a => a.Name).IsUnique();
            builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
            builder.HasMany(t => t.BookInfos).WithOne(b => b.Title).HasForeignKey(b => b.TitleId);
        }
    }
}
