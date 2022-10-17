using Marketplace.Common.Architecture;
using Marketplace.Common.Guards;

namespace Marketplace.ApplicationCore.Domain.Organizations.Entities;

/// <summary>
/// Организация (группировка аккаунтов)
/// </summary>
public class Organization : Entity
{
    protected Organization()
    {
    }

    #region Factory

    /// <summary>
    /// Создаёт новый объект организации (группы аккаунтов)
    /// </summary>
    /// <param name="name">Наименование организации (группы аккаунтов)</param>
    public static Organization Create(string name)
    {
        Guard.String.IsNotNullOrWhitespace(name, "Organization name can't be empty!");
        
        return new Organization { Name = name };
    }

    #endregion

    #region Body

    /// <summary>
    /// Наименование организации (группы аккаунтов)
    /// </summary>
    public string Name { get; protected set; }

    #endregion

    #region Navigation

    /// <summary>
    /// <see cref="OrganizationUserLink">Пользователи</see> организации
    /// </summary>
    public IReadOnlyList<OrganizationUserLink> Users
    {
        get => users.AsReadOnly();
        protected set => users = value?.ToList() ?? new();
    }
    
    protected List<OrganizationUserLink> users = new();

    #endregion

    #region Methods

    /// <summary>
    /// Добавляет ссылку на пользователя в организацию
    /// </summary>
    /// <param name="userLink"></param>
    public void AddUser(OrganizationUserLink userLink)
    {
        Guard.Object.NotNull(userLink, "OrganizationUser object is null");
        Guard.Entities.AreTheSame(userLink.Organization, this, "User linked to another organization!");
        
        if(users.All(x => x.Id != userLink.Id))
            users.Add(userLink);
    }

    #endregion
}