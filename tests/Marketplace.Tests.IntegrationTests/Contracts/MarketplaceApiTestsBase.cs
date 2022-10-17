using Marketplace.Common.Architecture;
using Marketplace.Tests.IntegrationTests.ApiControllers;
using Marketplace.Tests.IntegrationTests.Fixtures;
using Refit;
using Xunit;

namespace Marketplace.Tests.IntegrationTests.Contracts;

/// <summary>
/// Базовый класс для всех тестов, связанных с тестированием публичного АПИ торговой площадки
/// </summary>
[Collection(MarketplaceCollectionDefinition.CollectionName)]
public abstract class MarketplaceApiTestsBase
{
    /// <inheritdoc cref="MarketplaceApiFixture"/>
    protected readonly MarketplaceApiFixture Fixture;
    
    /// <inheritdoc cref="MarketplaceApiFixture.Storage"/>
    protected IApplicationStorage Storage => Fixture.Storage;

    protected MarketplaceApiTestsBase(MarketplaceApiFixture fixture)
    {
        Fixture = fixture;
    }

    #region Controllers

    /// <inheritdoc cref="ISessionController"/>
    public ISessionController SessionController => RestService.For<ISessionController>(Fixture.Client);

    #endregion
}