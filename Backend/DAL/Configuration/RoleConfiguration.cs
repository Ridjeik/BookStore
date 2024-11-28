using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole { Id = "1", Name = "Member", NormalizedName = "Member".ToUpper() },
                new IdentityRole { Id = "2", Name = "Employee", NormalizedName = "Employee".ToUpper() },
                new IdentityRole { Id = "3", Name = "Admin", NormalizedName = "Admin".ToUpper() });
        }
    }
}
