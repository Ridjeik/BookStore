using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Available).IsRequired();
            builder.Property(b => b.NumberInRow).IsRequired();
            builder.Property(b => b.RowId).IsRequired();
            builder.Property(b => b.BookInfoId).IsRequired();

            builder.HasOne(b => b.Row)
                   .WithMany(e => e.Books)
                   .HasForeignKey(b => b.RowId);

            builder.HasOne(b => b.BookInfo)
                   .WithMany(bi => bi.Books)
                   .HasForeignKey(b => b.BookInfoId);

            builder.HasMany(b => b.Reservations)
               .WithOne(e => e.Book)
               .HasForeignKey(r => r.BookId);
        }
    }

}
