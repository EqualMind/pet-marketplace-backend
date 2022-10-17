using FluentValidation;
using Marketplace.ApplicationCore.Contracts.Encrypting;
using Marketplace.ApplicationCore.Contracts.JsonWebTokens;
using Marketplace.ApplicationCore.Domain.Accounts;
using Marketplace.ApplicationCore.Domain.Accounts.Contracts;
using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.Common;
using Marketplace.Common.Architecture;
using Marketplace.Common.ExceptionHandling;
using Marketplace.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ApplicationCore.Operations.Security;

/// <summary>
/// Операция авторизации в системе
/// </summary>
public static class Authorization
{
    /// <summary>
    /// Аргументы операции авторизации в системе
    /// </summary>
    /// <param name="Email">Адрес электронной почты</param>
    /// <param name="Password">Пароль</param>
    public record Arguments(string Email, string Password) : IOperationArguments<Result>;

    /// <summary>
    /// Результат выполнения операции авторизации в системе
    /// </summary>
    /// <param name="AccessToken">Токен доступа</param>
    /// <param name="RefreshTokenHash">Вычисленный хэш токена обновления доступа</param>
    public record Result(string AccessToken, string RefreshTokenHash);

    /// <summary>
    /// Обработчик операции авторизации в системе
    /// </summary>
    public class Operation : Operation<Arguments, Result>
    {
        private readonly IStringEncoder encoder;
        private readonly IValidator<Arguments> validator;
        private readonly IJsonWebTokenService tokenService;
        private readonly AccountManager accountManager;
        private readonly IApplicationStorageReader storageReader;
        private readonly ApplicationEventBus applicationEventBus;

        public Operation(
            IStringEncoder encoder,
            IValidator<Arguments> validator,
            IJsonWebTokenService tokenService,
            AccountManager accountManager,
            IApplicationStorageReader storageReader,
            ApplicationEventBus applicationEventBus
        )
        {
            this.encoder = encoder;
            this.validator = validator;
            this.tokenService = tokenService;
            this.accountManager = accountManager;
            this.storageReader = storageReader;
            this.applicationEventBus = applicationEventBus;
        }

        public override async Task<Result> ExecuteAsync(Arguments arguments)
        {
            using var transaction = SystemTransactionsFactory.Default();

            await validator.ValidateObjectAsync(arguments, "Incorrect authorization arguments");

            var foundCredentials = await storageReader.FindAll<Account>()
                .Where(account => account.Email == arguments.Email)
                .Select(account => new { account.Id })
                .FirstOrDefaultAsync();

            foundCredentials.ThrowIfNull(new ForbiddenException("Account not found. Access denied!"));

            var account = await accountManager.GetFilledAsync(foundCredentials!.Id);

            encoder
                .Compare(arguments.Password, account.PasswordHash)
                .ThrowIfFalse(new ForbiddenException("Wrong password. Access denied!"));

            var (accessToken, refreshToken) = tokenService.GenerateAccessTokens(account.Id);

            account.AddToken(AccountTokenType.RefreshToken, refreshToken);
            await accountManager.UpdateAsync(account);

            transaction.Complete();

            return new Result(accessToken, encoder.Hash(refreshToken));
        }
    }

    public class ArgumentsValidation : AbstractValidator<Arguments>
    {
        public ArgumentsValidation()
        {
            RuleFor(args => args.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(args => args.Password).NotEmpty();
        }
    }
}