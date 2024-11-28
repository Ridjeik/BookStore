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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.UserId);
            builder.Property(e => e.Salary);

            builder.HasOne(e => e.User)
               .WithOne(u => u.Employee)
               .HasForeignKey<Employee>(e => e.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ReservationCreator)
               .WithOne(b => b.Employee)
               .HasForeignKey(b => b.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
