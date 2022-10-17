namespace Marketplace.ApplicationCore.Domain.Organizations.Contracts;

/// <summary>
/// Роль пользователя в организации (группы аккаунтов)
/// </summary>
public enum OrganizationUserRole
{
    /// <summary>
    /// Администратор организации (группы аккаунтов)
    /// </summary>
    Admin = -1,
    
    /// <summary>
    /// Менеджер организации (группы аккаунтов)
    /// </summary>
    Manager = 2
}