namespace Marketplace.ApplicationCore.Domain.Accounts.Contracts;

/// <summary>
/// Тип аккаунта
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Администратор торговой площадки
    /// </summary>
    MarketplaceAdmin = -1,
    
    /// <summary>
    /// Индивидуальный аккаунт
    /// </summary>
    Individual = 0,
    
    /// <summary>
    /// Групповой аккаунт (привязан к организации)
    /// </summary>
    Organizational = 1
}