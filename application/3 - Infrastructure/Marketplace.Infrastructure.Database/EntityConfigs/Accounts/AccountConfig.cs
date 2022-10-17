using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.Common.Extensions;
using Marketplace.Infrastructure.Database.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Infrastructure.Database.EntityConfigs.Accounts;

public class AccountConfig : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("account_credentials", DbSchemas.Accounts);
        builder.ConfigureEntityDefaultFields();

        builder
            .HasMany(account => account.Tokens)
            .WithOne(token => token.Account)
            .HasForeignKey(token => token.AccountId);
        
        builder
            .Navigation(account => account.Tokens)
            .HasField(nameof(Account.Tokens).SetFirstLetterToLowerCase())
            .AutoInclude();

        builder
            .Navigation(account => account.OrganizationLink)
            .AutoInclude();
    }
}