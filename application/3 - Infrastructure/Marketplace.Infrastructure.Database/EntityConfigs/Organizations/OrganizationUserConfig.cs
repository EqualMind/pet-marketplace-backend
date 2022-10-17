using Marketplace.ApplicationCore.Domain.Organizations.Entities;
using Marketplace.Common.Extensions;
using Marketplace.Infrastructure.Database.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Database.EntityConfigs.Organizations;

public class OrganizationUserConfig : IEntityTypeConfiguration<OrganizationUserLink>
{
    public void Configure(EntityTypeBuilder<OrganizationUserLink> builder)
    {
        builder.ToTable("organization_users", DbSchemas.Organizations);
        builder.ConfigureEntityDefaultFields();

        builder
            .HasOne(user => user.Account)
            .WithOne(account => account.OrganizationLink)
            .HasForeignKey<OrganizationUserLink>(user => user.AccountId);

        builder
            .Navigation(user => user.Account)
            .AutoInclude();

        builder
            .Navigation(user => user.Organization)
            .AutoInclude();
    }
}