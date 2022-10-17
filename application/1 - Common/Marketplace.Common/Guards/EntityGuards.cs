using Marketplace.Common.Architecture;
using Marketplace.Common.ExceptionHandling;

namespace Marketplace.Common.Guards;

public static partial class Guard
{
    public static class Entities
    {
        /// <summary>
        /// Осуществляет проверку что оба указанных объекта сущности имеют одинаковый идентификатор
        /// </summary>
        /// <param name="firstEntity">Первый объект сущности</param>
        /// <param name="secondEntity">Второй объект сущности</param>
        /// <param name="message">Сообщение об ошибке</param>
        public static void AreTheSame<TEntity>(TEntity firstEntity, TEntity secondEntity, string message) where TEntity : Entity
        {
            if (firstEntity.Id != secondEntity.Id)
                throw new GuardValidationException(message);
        }
    }
}