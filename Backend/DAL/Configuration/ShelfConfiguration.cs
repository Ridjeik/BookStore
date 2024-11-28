using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configuration
{
    public class ShelfConfiguration : IEntityTypeConfiguration<Shelf>
    {
        public void Configure(EntityTypeBuilder<Shelf> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasMany(s => s.ShelfGenre)
                   .WithOne(t => t.Shelf)
                   .HasForeignKey(t => t.ShelfId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Room)
                   .WithMany(r => r.Shelfs)
                   .HasForeignKey(s => s.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Rows)
                   .WithOne(r => r.Shelf)
                   .HasForeignKey(r => r.ShelfId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
