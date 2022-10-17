namespace Marketplace.Common.Architecture;

/// <summary>
/// Аргументы бизнес-операции
/// </summary>
public interface IOperationArguments
{
}

/// <summary>
/// Аргументы бизнес-операции с указанием типа возвращаемого результата
/// </summary>
/// <typeparam name="TResult">Тип возвращаемого результата</typeparam>
public interface IOperationArguments<TResult> where TResult : class
{
}

/// <summary>
/// Всегда пустой набор аргументов, на случай, если операция не содержит входных данных
/// </summary>
public sealed class OperationEmptyArguments : IOperationArguments
{
}

/// <summary>
/// Всегда пустой набор аргументов, на случай, если операция не содержит входных данных
/// </summary>
/// <typeparam name="TResult">Возвращаемый результат</typeparam>
public sealed class OperationEmptyArguments<TResult> : IOperationArguments<TResult> where TResult : class
{
}