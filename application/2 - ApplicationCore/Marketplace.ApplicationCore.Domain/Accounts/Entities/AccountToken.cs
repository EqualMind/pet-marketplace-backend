using Marketplace.ApplicationCore.Domain.Accounts.Contracts;
using Marketplace.Common.Architecture;
using Marketplace.Common.Guards;

namespace Marketplace.ApplicationCore.Domain.Accounts.Entities;

/// <summary>
/// Токен аккаунта
/// </summary>
public class AccountToken : Entity
{
    protected AccountToken()
    {
        
    }

    #region Factories

    /// <summary>
    /// Создаёт новый объект, хранящий токен для аккаунта 
    /// </summary>
    /// <param name="account">Аккаунт, которому принадлежит токен</param>
    /// <param name="type">Тип токена</param>
    /// <param name="body">Тело токена</param>
    public static AccountToken Create(Account account, AccountTokenType type, string body)
    {
        Guard.Object.NotNull(account, $"({nameof(AccountToken)}) Account entity can't be null!");
        Guard.String.IsNotNullOrWhitespace(body, $"({nameof(AccountToken)}) Token body can't be empty!");

        return new AccountToken
        {
            AccountId = account.Id, 
            Type = type, 
            Body = body,
            Account = account
        };
    }

    #endregion

    #region Body
    
    /// <summary>
    /// Идентификатор аккаунта которому принадлежит токен
    /// </summary>
    public Guid AccountId { get; protected set; }

    /// <inheritdoc cref="AccountTokenType"/>
    public AccountTokenType Type { get; protected set; }
    
    /// <summary>
    /// Тело токена
    /// </summary>
    public string Body { get; protected set; }

    #endregion

    #region Navigation

    /// <summary>
    /// Аккаунт к которому привязан токен
    /// </summary>
    public Account Account { get; protected set; }

    #endregion
}