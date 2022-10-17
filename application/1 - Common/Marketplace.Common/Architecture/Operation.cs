namespace Marketplace.Common.Architecture;

/// <summary>
/// Метка бизнес-операции для механизма инъекции зависимостей
/// </summary>
public interface IOperation
{
}

/// <summary>
/// Бизнес-операция с указанными аргументами
/// </summary>
/// <typeparam name="TArguments"></typeparam>
public abstract class Operation<TArguments> : IOperation where TArguments : class, IOperationArguments
{
    /// <summary>
    /// Осуществляет выполнение операции
    /// </summary>
    /// <param name="arguments">Аргументы операции</param>
    public abstract Task ExecuteAsync(TArguments arguments);
}

/// <summary>
/// Бизнес-операция с указанными аргументами и возвращаемым результатом
/// </summary>
/// <typeparam name="TArguments">Тип аргументов</typeparam>
/// <typeparam name="TResult">Тип возвращаемого результата</typeparam>
public abstract class Operation<TArguments, TResult> : IOperation
    where TArguments : class, IOperationArguments<TResult>
    where TResult : class
{
    /// <summary>
    /// Осуществляет выполнение операции с возвратом результата
    /// </summary>
    /// <param name="arguments">Аргументы операции</param>
    public abstract Task<TResult> ExecuteAsync(TArguments arguments);
}