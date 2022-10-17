using Marketplace.ApplicationCore.Contracts.Encrypting;
using Marketplace.ApplicationCore.Domain.Accounts.Contracts;
using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.Common.Architecture;
using Marketplace.Infrastructure.Database.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Marketplace.Infrastructure.Database.Seeding;

/// <summary>
/// Создаёт начальный аккаунт администратора в базе данных
/// </summary>
public sealed class AddAdminAccountSeedSpecification : SeedSpecification
{
    private readonly IStringEncoder encoder;

    private const string Email = PredefinedConstants.Admin.Email;
    private const string Password = PredefinedConstants.Admin.Password;
    
    public AddAdminAccountSeedSpecification(IStringEncoder encoder)
    {
        this.encoder = encoder;
    }

    public override async Task<bool> CanBeAppliedAsync(IApplicationStorageReader storage, IConfiguration config, IWebHostEnvironment env)
        => !await storage.FindAll<Account>()
            .Where(account => account.Email == Email)
            .AnyAsync();

    public override async Task ApplyAsync(IApplicationStorage storage, IConfiguration config, IWebHostEnvironment env)
    {
        var passwordHash = encoder.Hash(Password);
        var admin = Account.Create(Email, passwordHash, AccountType.MarketplaceAdmin);
        
        await storage.AddAsync(admin);
    }
}