namespace Marketplace.Infrastructure.Database.Contracts;

/// <summary>
/// Предопределенные константы для работы с базой и данными
/// </summary>
public static class PredefinedConstants
{
    /// <summary>
    /// Данные учетной записи администратора
    /// </summary>
    public static class Admin
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public const string Email = "marketplace-admin@local.ru";
        
        /// <summary>
        /// Пароль
        /// </summary>
        public const string Password = "P@ssw0rd";
    }
}