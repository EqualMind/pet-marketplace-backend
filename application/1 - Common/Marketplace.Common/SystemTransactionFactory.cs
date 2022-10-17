using System.Transactions;

namespace Marketplace.Common;

/// <summary>
/// Генератор транзакций
/// </summary>
public static class SystemTransactionsFactory
{
    /// <summary>Создаёт <see cref="TransactionScope"/> по умолчанию с опцией TransactionScopeOption.<see cref="TransactionScopeOption.Required"/></summary>
    /// <seealso cref="TransactionScope"/>
    public static TransactionScope Default() => new(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

    /// <summary>Создаёт настраиваемый <see cref="TransactionScope"/></summary>
    /// <param name="option"><see cref="TransactionScopeOption"/></param>
    public static TransactionScope New(TransactionScopeOption option) => new(option, TransactionScopeAsyncFlowOption.Enabled);
}