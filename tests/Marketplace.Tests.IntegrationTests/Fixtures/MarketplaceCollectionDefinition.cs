using Xunit;

namespace Marketplace.Tests.IntegrationTests.Fixtures;

[CollectionDefinition(CollectionName)]
public class MarketplaceCollectionDefinition : ICollectionFixture<MarketplaceApiFixture>
{
    public const string CollectionName = "Marketplace Api";
}