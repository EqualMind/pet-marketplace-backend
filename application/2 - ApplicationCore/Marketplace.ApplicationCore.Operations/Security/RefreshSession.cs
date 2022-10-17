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
using Marketplace.Common.TypesValidation;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ApplicationCore.Operations.Security;

/// <summary>
/// Обновление сессии в системе
/// </summary>
public static class RefreshSession
{
    /// <summary>
    /// Аргументы для обновления сессии в системе
    /// </summary>
    /// <param name="UserId">Идентификатор аккаунта пользователя</param>
    /// <param name="RefreshToken">Токен обновления сессии</param>
    /// <param name="RefreshTokenHash">Хэшированная строка токена обновления сессии</param>
    public record Arguments(string? UserId, string? RefreshToken, string? RefreshTokenHash) : IOperationArguments<Result>;

    /// <summary>
    /// Результат обновления сессии (новая пара токена доступа и хэша токена обновления сессии)
    /// </summary>
    /// <param name="AccessToken">Новый токен доступа</param>
    /// <param name="RefreshTokenHash">Новый хэш токена обновления сессии</param>
    public record Result(string AccessToken, string RefreshTokenHash);

    /// <summary>
    /// Обработчик операции обновления сессии в системе
    /// </summary>
    public class Operation : Operation<Arguments, Result>
    {
        private readonly IValidator<Arguments> validator;
        private readonly AccountManager accountManager;
        private readonly IApplicationStorageReader storageReader;
        private readonly IJsonWebTokenService tokenService;
        private readonly IStringEncoder encoder;

        public Operation(
            IValidator<Arguments> validator,
            AccountManager accountManager,
            IApplicationStorageReader storageReader,
            IJsonWebTokenService tokenService,
            IStringEncoder encoder
        )
        {
            this.validator = validator;
            this.accountManager = accountManager;
            this.storageReader = storageReader;
            this.tokenService = tokenService;
            this.encoder = encoder;
        }

        public override async Task<Result> ExecuteAsync(Arguments arguments)
        {
            using var transaction = SystemTransactionsFactory.Default();

            await ValidateCredentials(arguments);

            var accountId = Guid.Parse(arguments.UserId!);
            var accountExists = await storageReader.FindAll<Account>().AnyAsync(account => account.Id == accountId);
            accountExists.ThrowIfFalse(new ForbiddenException("Аккаунт не найден"));

            var account = await accountManager.GetFilledAsync(accountId);

            var accountRefreshToken = account.Tokens.FirstOrDefault(token => token.Body == arguments.RefreshToken);
            accountRefreshToken.ThrowIfNull(new ForbiddenException("Токена обновления сессии не существует"));

            var refreshTokenIsValid = encoder.Compare(accountRefreshToken!.Body, arguments.RefreshTokenHash!);
            account.RemoveToken(accountRefreshToken);
            
            if (!refreshTokenIsValid)
            {
                await accountManager.UpdateAsync(account);
                transaction.Complete();
                throw new ForbiddenException("Невалидный токен обновления сессии");
            }
            
            var (accessToken, refreshToken) = tokenService.GenerateAccessTokens(accountId);
            account.AddToken(AccountTokenType.RefreshToken, refreshToken);
            
            await accountManager.UpdateAsync(account);
            transaction.Complete();

            return new Result(accessToken, encoder.Hash(refreshToken));
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

            RuleFor(args => args.RefreshTokenHash)
                .NotEmpty();
        }
    }
}