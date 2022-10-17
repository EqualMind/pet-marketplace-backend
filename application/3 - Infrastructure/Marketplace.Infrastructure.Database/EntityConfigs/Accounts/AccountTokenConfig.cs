using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.Common.Extensions;
using Marketplace.Infrastructure.Database.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Database.EntityConfigs.Accounts;

public class AccountTokenConfig : IEntityTypeConfiguration<AccountToken>
{
    public void Configure(EntityTypeBuilder<AccountToken> builder)
    {
        builder.ToTable("account_tokens", DbSchemas.Accounts);
        builder.ConfigureEntityDefaultFields();
        
        builder
            .Navigation(token => token.Account)
            .AutoInclude();
    }
}