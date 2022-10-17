using Marketplace.ApplicationCore.Domain.Accounts.Contracts;
using Marketplace.ApplicationCore.Domain.Organizations.Entities;
using Marketplace.Common.Architecture;
using Marketplace.Common.Guards;

namespace Marketplace.ApplicationCore.Domain.Accounts.Entities;

/// <summary>
/// Учетные данные пользователя
/// </summary>
public class Account : Entity
{
    protected Account()
    {
        
    }

    #region Factory
    
    /// <summary>
    /// Создаёт новый объект учетных данных пользователя
    /// </summary>
    /// <param name="email">Адрес электронной почты</param>
    /// <param name="passwordHash">Хешированный пароль</param>
    /// <param name="type">Тип аккаунта (По умолчанию: <see cref="AccountType.Individual"/>)</param>
    public static Account Create(string email, string passwordHash, AccountType type = AccountType.Individual)
    {
        Guard.String.IsNotNullOrWhitespace(email, "Email can't be empty!");
        Guard.String.IsNotNullOrWhitespace(passwordHash, "Password hash can't be empty!");
        Guard.String.IsEmail(email, "Invalid email address!");
        
        return new()
        {
            Email = email, 
            PasswordHash = passwordHash,
            Type = type
        };
    }

    #endregion

    #region Body

    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string Email { get; protected set; }
    
    /// <summary>
    /// Строка с вычесленным хэшем пароля
    /// </summary>
    public string PasswordHash { get; protected set; }
    
    /// <inheritdoc cref="AccountType"/>
    public AccountType Type { get; protected set; }

    #endregion

    #region Navigation
    
    /// <summary>
    /// Набор сгенерированных токенов аккаунта
    /// </summary>
    public IReadOnlyList<AccountToken> Tokens
    {
        get => tokens.AsReadOnly();
        protected set => tokens = value?.ToList() ?? new();
    }

    protected List<AccountToken> tokens = new();
    
    /// <summary>
    /// <see cref="OrganizationUserLink">Связка с организацией</see> к которой принадлежит аккаунт
    /// </summary>
    /// <remarks>
    /// Присутствует только у <see cref="AccountType.Organizational" /> аккаунтов
    /// </remarks>
    public OrganizationUserLink? OrganizationLink { get; protected set; }

    #endregion

    #region Methods

    /// <summary>
    /// Устанавливает новый хэш пароля для учетных данных
    /// </summary>
    /// <param name="passwordHash">Новый хэш</param>
    public void ChangePasswordHash(string passwordHash)
    {
        Guard.String.IsNotNullOrWhitespace(passwordHash, "Password hash can't be empty!");
        
        PasswordHash = passwordHash;
    }

    /// <summary>
    /// Добавляет новый токен к аккаунту
    /// </summary>
    /// <param name="type">Тип токена</param>
    /// <param name="body">Тело токена</param>
    public void AddToken(AccountTokenType type, string body)
    {
        Guard.String.IsNotNullOrWhitespace(body, "Token body can't be empty!");
        
        tokens.Add(AccountToken.Create(this, type, body));
    }

    /// <summary>
    /// Добавляет новый токен к аккаунту
    /// </summary>
    /// <param name="token">Токен</param>
    public void AddToken(AccountToken token)
    {
        Guard.Object.NotNull(token, "Token is null!");
        Guard.Entities.AreTheSame(this, token.Account, "Account linked with token have different id!");
        
        tokens.Add(token);
    }
    
    /// <summary>
    /// Удаляет запись о токене аккаунта
    /// </summary>
    /// <param name="token">Токен</param>
    public void RemoveToken(AccountToken token)
    {
        Guard.Object.NotNull(token, "Token is null!");
        Guard.Entities.AreTheSame(this, token.Account, "Account linked with token have different id!");

        tokens.Remove(token);
    }
    
    /// <summary>
    /// Осуществляет привязку аккаунта к организации по указанной связке
    /// </summary>
    /// <param name="link">Связка с организацией</param>
    public void ConvertToOrganizational(OrganizationUserLink link)
    {
        Guard.Object.NotNull(link, "Organization link is null!");
        Guard.Entities.AreTheSame(link.Account, this, "Linked Account is different!");

        Type = AccountType.Organizational;
        OrganizationLink = link;
    }

    #endregion
}