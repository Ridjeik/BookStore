using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace DAL.Configuration
{
    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.HasKey(bt => new { bt.BookInfoId, bt.GenreId });

            builder.HasOne(bt => bt.BookInfo)
                .WithMany(b => b.BookGenre)
                .HasForeignKey(bt => bt.BookInfoId);

            builder.HasOne(bt => bt.Genre)
                .WithMany(t => t.BookGenres)
                .HasForeignKey(bt => bt.GenreId);
        }
    }
}
