# CleanTemplate
Clean Architecture + DDD + CQRS + Domain Events + Testing

## Docker
`docker-compose up` @ project root

## Migrations
`dotnet ef migrations add <migration_name> --project .\Infrastructure\Infrastructure.csproj --startup-project .\WebApi\WebApi.csproj`

## Database
`dotnet ef database update --project .\Infrastructure\Infrastructure.csproj --startup-project .\WebApi\WebApi.csproj`

<sub>There's stuff that shoud be private kept public on purpose, like `appsettings.json` and `Docker` directory, for discoverability.</sub>
