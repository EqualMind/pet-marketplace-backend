using FluentValidation;
using Marketplace.ApplicationCore.Contracts.Encrypting;
using Marketplace.ApplicationCore.Domain.Accounts;
using Marketplace.ApplicationCore.Domain.Accounts.Contracts;
using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.Common.Architecture;
using Marketplace.Common.ExceptionHandling;
using Marketplace.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ApplicationCore.Operations.AccountManagement;

/// <summary>
/// Операция по создания аккаунта пользователя
/// </summary>
public static class CreateAccount
{
    /// <summary>
    /// Создаёт новый аккаунт пользователя в системе
    /// </summary>
    /// <param name="Email">Адрес электронной почты</param>
    /// <param name="Password">Пароль</param>
    /// <param name="AccountType">Тип аккаунта</param>
    public record Arguments(string Email, string Password, AccountType AccountType = AccountType.Individual) : IOperationArguments;

    /// <summary>
    /// Обработчик операции по созданию аккаунта пользователя
    /// </summary>
    public class Operation : Operation<Arguments>
    {
        private readonly IValidator<Arguments> validator;
        private readonly IApplicationStorageReader storageReader;
        private readonly AccountManager accountManager;
        private readonly IStringEncoder stringEncoder;

        public Operation(
            IValidator<Arguments> validator,
            IApplicationStorageReader storageReader,
            AccountManager accountManager,
            IStringEncoder stringEncoder
        )
        {
            this.validator = validator;
            this.storageReader = storageReader;
            this.accountManager = accountManager;
            this.stringEncoder = stringEncoder;
        }

        public override async Task ExecuteAsync(Arguments arguments)
        {
            await validator.ValidateObjectAsync(arguments, "Некорректные данные для создания пользователя!");

            var accountExists = await storageReader.FindAll<Account>()
                .AnyAsync(account => account.Email == arguments.Email);

            accountExists.ThrowIfTrue(new BadRequestException("Аккаунт с таким адресом электронной почты уже есть в системе"));

            var hashedPassword = stringEncoder.Hash(arguments.Password);
            var account = Account.Create(arguments.Email, hashedPassword, arguments.AccountType);

            await accountManager.CreateAsync(account);
        }
    }

    public class ArgumentsValidation : AbstractValidator<Arguments>
    {
        public ArgumentsValidation()
        {
            RuleFor(args => args.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(args => args.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}