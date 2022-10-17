using FluentValidation;
using Marketplace.ApplicationCore.Domain.Accounts;
using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.Common;
using Marketplace.Common.Architecture;
using Marketplace.Common.ExceptionHandling;
using Marketplace.Common.Extensions;
using Marketplace.Common.TypesValidation;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ApplicationCore.Operations.Security;

/// <summary>
/// Выход из системы
/// </summary>
public static class Logout
{
    /// <summary>
    /// Аргументы для операции выхода из системы
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя</param>
    /// <param name="RefreshToken">Токен обновления доступа</param>
    public record Arguments(string? UserId, string? RefreshToken) : IOperationArguments;
    
    /// <summary>
    /// Операция выхода из системы
    /// </summary>
    public class Operation : Operation<Arguments>
    {
        private readonly IValidator<Arguments> validator;
        private readonly IApplicationStorageReader storageReader;
        private readonly AccountManager accountManager;

        public Operation(IValidator<Arguments> validator, IApplicationStorageReader storageReader, AccountManager accountManager)
        {
            this.validator = validator;
            this.storageReader = storageReader;
            this.accountManager = accountManager;
        }

        public override async Task ExecuteAsync(Arguments arguments)
        {
            using var transaction = SystemTransactionsFactory.Default();

            await ValidateCredentials(arguments);
            
            var accountId = Guid.Parse(arguments.UserId!);
            var accountExists = await storageReader.FindAll<Account>().AnyAsync(account => account.Id == accountId);
            accountExists.ThrowIfFalse(new ForbiddenException("Аккаунт не найден"));

            var account = await accountManager.GetFilledAsync(accountId);
            var accountRefreshToken = account.Tokens.FirstOrDefault(token => token.Body == arguments.RefreshToken);

            if (accountRefreshToken != null)
            {
                account.RemoveToken(accountRefreshToken);
                await accountManager.UpdateAsync(account);
            }

            transaction.Complete();
        }
        
        private async Task ValidateCredentials(Arguments arguments)
        {
            try
            {
                await validator.ValidateObjectAsync(arguments, string.Empty);
            }
            catch (CommonValidationException e)
            {
                throw new ForbiddenException("Данные для обновления сессии повреждены! Авторизуйтесь снова!");
            }
        }
    }

    public class ArgumentsValidation : AbstractValidator<Arguments>
    {
        public ArgumentsValidation()
        {
            RuleFor(args => args.UserId)
                .NotEmpty()
                .Must(StringValidators.IsGuid);

            RuleFor(args => args.RefreshToken)
                .NotEmpty();
        }
    }
}