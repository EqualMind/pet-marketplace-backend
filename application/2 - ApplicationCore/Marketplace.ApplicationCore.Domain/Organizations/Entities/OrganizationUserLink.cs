using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.ApplicationCore.Domain.Organizations.Contracts;
using Marketplace.Common.Architecture;
using Marketplace.Common.Guards;

namespace Marketplace.ApplicationCore.Domain.Organizations.Entities;

/// <summary>
/// Связка аккаунта с организацией
/// </summary>
public class OrganizationUserLink : Entity
{
    protected OrganizationUserLink()
    {
        
    }

    #region Factory

    /// <summary>
    /// Создаёт новый объект пользователя организации
    /// </summary>
    /// <param name="organization">Организация</param>
    /// <param name="account">Аккаунт пользователя</param>
    /// <param name="role">Роль пользователя в данной организации (По умолчанию <see cref="OrganizationUserRole.Manager"/>)</param>
    public static OrganizationUserLink Create(Organization organization, Account account, OrganizationUserRole role = OrganizationUserRole.Manager)
    {
        Guard.Object.NotNull(organization, "Organization object is null!");
        Guard.Object.NotNull(account, "Account object is null!");
        
        return new OrganizationUserLink
        {
            OrganizationId = organization.Id,
            AccountId = account.Id,
            Organization = organization,
            Account = account,
            Role = role
        };
    }

    #endregion
    
    #region Body

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; protected set; }

    /// <summary>
    /// Идентификатор аккаунта
    /// </summary>
    public Guid AccountId { get; protected set; }

    /// <inheritdoc cref="OrganizationUserRole"/>
    public OrganizationUserRole Role { get; protected set; }
    
    #endregion

    #region Navigation

    /// <summary>
    /// Организация к которой принадлежит аккаунт
    /// </summary>
    public Organization Organization { get; protected set; }

    /// <summary>
    /// Аккаунт пользователя организации
    /// </summary>
    public Account Account { get; protected set; }
    
    #endregion
}