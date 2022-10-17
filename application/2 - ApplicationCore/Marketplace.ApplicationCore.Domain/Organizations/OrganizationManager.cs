using Marketplace.ApplicationCore.Domain.Organizations.Entities;
using Marketplace.Common;
using Marketplace.Common.Architecture;
using Marketplace.Common.Guards;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ApplicationCore.Domain.Organizations;

public class OrganizationManager : DomainManager<Organization>
{
    /// <summary>
    /// Регистрирует новый объект сущности организации (группы аккаунтов) в системе
    /// </summary>
    /// <param name="organization">Объект организации</param>
    public override async Task CreateAsync(Organization organization)
    {
        using var transaction = SystemTransactionsFactory.Default();
        
        Guard.Object.NotNull(organization, "Organization object is null!");

        await Storage.AddAsync(organization);
        await Storage.AddRangeAsync(organization.Users);
        
        transaction.Complete();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organization">Объект организации</param>
    public override async Task UpdateAsync(Organization organization)
    {
        using var transaction = SystemTransactionsFactory.Default();

        Guard.Object.NotNull(organization, "Organization object is null!");
        
        await Storage.UpdateAsync(organization);
        await UpdateOrganizationUsers(organization);
        
        transaction.Complete();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organization">Объект организации</param>
    public override async Task DeleteAsync(Organization organization)
    {
        using var transaction = SystemTransactionsFactory.Default();

        Guard.Object.NotNull(organization, "Organization object is null!");
        
        await Storage.DeleteRangeAsync(organization.Users);
        await Storage.DeleteAsync(organization);
        
        transaction.Complete();
    }
    
    /// <summary>
    /// Алгоритм обновления списка пользователей организации
    /// </summary>
    /// <param name="organization">Организация</param>
    private async Task UpdateOrganizationUsers(Organization organization)
    {
        var databaseOrganizationUsers = await Storage.ExtractEntities<OrganizationUserLink>()
            .Where(link => link.OrganizationId == organization.Id)
            .ToListAsync();

        var databaseOrganizationUsersIds = databaseOrganizationUsers.Select(link => link.Id).ToList();
        var currentOrganizationUsersIds = organization.Users.Select(link => link.Id).ToList();

        var addedOrganizationUsers = organization.Users.Where(link => !databaseOrganizationUsersIds.Contains(link.Id)).ToList();
        var deletedOrganizationUsers = databaseOrganizationUsers.Where(link => !currentOrganizationUsersIds.Contains(link.Id)).ToList();
        var updatedOrganizationUsers = organization.Users.Where(link => databaseOrganizationUsersIds.Contains(link.Id)).ToList();

        await Storage.AddRangeAsync(addedOrganizationUsers);
        await Storage.UpdateRangeAsync(updatedOrganizationUsers);
        await Storage.DeleteRangeAsync(deletedOrganizationUsers);
    }
    
}