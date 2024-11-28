using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.EventType).IsRequired();

            builder.HasOne(e => e.Reservation)
                   .WithMany(r => r.Events)
                   .HasForeignKey(e => e.ReservationId);
        }
    }
}