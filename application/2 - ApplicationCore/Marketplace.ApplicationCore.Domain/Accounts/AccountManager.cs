using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.Common;
using Marketplace.Common.Architecture;
using Marketplace.Common.Guards;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ApplicationCore.Domain.Accounts;

/// <summary>
/// Менеджер аккаунтов
/// </summary>
public class AccountManager : DomainManager<Account>
{
    /// <summary>
    /// Регистрирует новый аккаунт в системе
    /// </summary>
    /// <param name="account">Новая учетная запись</param>
    public override async Task CreateAsync(Account account)
    {
        using var transaction = SystemTransactionsFactory.Default();
        
        Guard.Object.NotNull(account, "Account object is null!");
        
        await Storage.AddAsync(account);
        await Storage.AddRangeAsync(account.Tokens);
        
        transaction.Complete();
    }

    /// <summary>
    /// Обновляет информцию об аккаунте пользователя
    /// </summary>
    /// <param name="account">Объект учетной записи для обновления</param>
    public override async Task UpdateAsync(Account account)
    {
        using var transaction = SystemTransactionsFactory.Default();
        
        Guard.Object.NotNull(account, "Account object is null!");
        
        await Storage.UpdateAsync(account);
        await UpdateTokensAsync(account);
        
        transaction.Complete();
    }
    
    /// <summary>
    /// Осуществляет алгоритм удаления аккаунта и информации связанной с ним
    /// </summary>
    /// <remarks>
    /// Использовать осторожно!!
    /// </remarks>
    /// <param name="account">Объект для удаления</param>
    public override async Task DeleteAsync(Account account)
    {
        using var transaction = SystemTransactionsFactory.Default();
        
        Guard.Object.NotNull(account, "Account object is null!");
        
        if (account.OrganizationLink != null)
            await Storage.DeleteAsync(account.OrganizationLink);

        await Storage.DeleteRangeAsync(account.Tokens);
        await Storage.DeleteAsync(account);
        
        transaction.Complete();
    }
    
    /// <summary>
    /// Алгоритм обновления набора токенов
    /// </summary>
    /// <param name="account">Аккаунт</param>
    private async Task UpdateTokensAsync(Account account)
    {
        var databaseTokens = await Storage.ExtractEntities<AccountToken>()
            .Where(token => token.AccountId == account.Id)
            .ToListAsync();

        var databaseTokensIds = databaseTokens.Select(token => token.Id).ToList();
        var currentTokensIds = account.Tokens.Select(token => token.Id).ToList();
        
        var addedTokens = account.Tokens.Where(token => !databaseTokensIds.Contains(token.Id)).ToList();
        var deletedTokens = databaseTokens.Where(token => !currentTokensIds.Contains(token.Id)).ToList();
        var updatedTokens = account.Tokens.Where(token => databaseTokensIds.Contains(token.Id)).ToList();

        await Storage.AddRangeAsync(addedTokens);
        await Storage.UpdateRangeAsync(updatedTokens);
        await Storage.DeleteRangeAsync(deletedTokens);
    }
}