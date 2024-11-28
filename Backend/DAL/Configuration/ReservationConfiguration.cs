using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Description).IsRequired();

            builder.HasOne(r => r.Book)
                    .WithMany(b => b.Reservations)
                    .HasForeignKey(r => r.BookId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.Events)
                   .WithOne(e => e.Reservation)
                   .HasForeignKey(e => e.ReservationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Member)
                   .WithMany(m => m.Reservations)
                   .HasForeignKey(r => r.MemberId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Employee)
                   .WithMany(e => e.ReservationCreator)
                   .HasForeignKey(r => r.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}