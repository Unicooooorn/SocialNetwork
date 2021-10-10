using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Account;
using SocialNetwork.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Data.EntityTypeConfiguration
{
    public class AccountConfiguration : IEntityTypeConfiguration<CreateAccount>
    {
        public void Configure(EntityTypeBuilder<CreateAccount> builder)
        {
            builder.HasKey(profile => profile.Id);
            builder.HasIndex(profile => profile.Id).IsUnique();
            builder.Property(profile => profile.FirstName).IsRequired();
            builder.Property(profile => profile.LastName).IsRequired();
            builder.Property(profile => profile.Age).IsRequired();
            builder.Property(profile => profile.Password).IsRequired();
        }
    }
}
