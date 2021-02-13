using AuthServer.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AuthServer.Data.Concrete.EntityFrameworkCore.Configurations
{
    internal class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.HasKey(p => p.UserId);
            builder.Property(p => p.Code).IsRequired().HasMaxLength(200);
        }
    }
}
