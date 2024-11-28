using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class RowConfiguration : IEntityTypeConfiguration<Row>
    {
        public void Configure(EntityTypeBuilder<Row> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Number).IsRequired();

            builder.HasOne(r => r.Shelf)
                   .WithMany(s => s.Rows)
                   .HasForeignKey(r => r.ShelfId);

            builder.HasMany(r => r.Books)
                   .WithOne(p => p.Row)
                   .HasForeignKey(p => p.RowId);
        }
    }
}
