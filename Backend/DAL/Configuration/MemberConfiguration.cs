using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(m => m.UserId);
            builder.Property(m => m.Balance);

            builder.HasOne(m => m.User)
               .WithOne(u => u.Member)
               .HasForeignKey<Member>(m => m.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Reservations)
                   .WithOne(r => r.Member)
                   .HasForeignKey(r => r.MemberId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }


}

