using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace DAL.Configuration
{
    public class ShelfGenreConfiguration : IEntityTypeConfiguration<ShelfGenre>
    {
        public void Configure(EntityTypeBuilder<ShelfGenre> builder)
        {
            builder.HasKey(st => new { st.ShelfId, st.GenreId });

            builder.HasOne(st => st.Shelf)
                .WithMany(s => s.ShelfGenre)
                .HasForeignKey(st => st.ShelfId);

            builder.HasOne(st => st.Genre)
                .WithMany(t => t.ShelfGenres)
                .HasForeignKey(st => st.GenreId);
        }
    }
}
