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
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).IsRequired();
            builder.HasIndex(a => a.Name).IsUnique();

            builder.HasMany(r => r.Shelfs)
                   .WithOne(s => s.Room)
                   .HasForeignKey(s => s.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
