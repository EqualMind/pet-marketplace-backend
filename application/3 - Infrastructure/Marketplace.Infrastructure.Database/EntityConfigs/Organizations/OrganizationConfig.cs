using Marketplace.ApplicationCore.Domain.Organizations.Entities;
using Marketplace.Common.Extensions;
using Marketplace.Infrastructure.Database.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Database.EntityConfigs.Organizations;

public class OrganizationConfig : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations", DbSchemas.Organizations);
        builder.ConfigureEntityDefaultFields();

        builder
            .HasMany(organization => organization.Users)
            .WithOne(user => user.Organization)
            .HasForeignKey(user => user.OrganizationId);

        builder
            .Navigation(organization => organization.Users)
            .HasField(nameof(Organization.Users).SetFirstLetterToLowerCase())
            .AutoInclude();
    }
}