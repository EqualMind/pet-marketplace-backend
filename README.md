# Marketplace

Добавить миграцию:

```shell
dotnet ef migrations add -c MarketplaceContext -s "./application/4 - Interaction/Marketplace.Interaction.Api" -p "./application/3 - Infrastructure/Marketplace.Infrastructure.Database"  {MigrationName}
```