using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace DAL.Configuration
{
    public class BookInfoConfiguration : IEntityTypeConfiguration<BookInfo>
    {
        public void Configure(EntityTypeBuilder<BookInfo> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.DatePublication)
                .IsRequired();

            builder.Property(b => b.PageNumber)
                .IsRequired()
                .HasDefaultValue(0)
                .HasAnnotation("MinLength", 1);

            builder.Property(b => b.Description)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(b => b.Price)
                .IsRequired()
                .HasDefaultValue(0.0)
                .HasAnnotation("MinLength", 0.01);

            builder.Property(b => b.Rating)
                .IsRequired()
                .HasDefaultValue(0)
                .HasAnnotation("Range", new[] { 0, 5 });

            builder.HasOne(b => b.Title)
                .WithMany(e => e.BookInfos)
                .HasForeignKey(b => b.TitleId)
                .IsRequired();

            builder.HasOne(b => b.Author)
                .WithMany(e => e.BookInfos)
                .HasForeignKey(b => b.AuthorId)
                .IsRequired();

            builder.HasOne(b => b.Publisher)
                .WithMany(e => e.BookInfos)
                .HasForeignKey(b => b.PublisherId)
                .IsRequired();

            builder.HasOne(b => b.Picture)
                .WithMany(e => e.BookInfos)
                .HasForeignKey(b => b.PictureId)
                .IsRequired();

            builder.HasMany(b => b.BookGenre)
                .WithOne(e => e.BookInfo)
                .HasForeignKey(bg => bg.BookInfoId)
                .IsRequired();

            builder.HasMany(b => b.Books)
               .WithOne(b => b.BookInfo)
               .HasForeignKey(b => b.BookInfoId)
               .IsRequired();
        }

    }
}
