#!/bin/sh

dotnet pack ./PlaygroundShared.Api/PlaygroundShared.Api.csproj -o ./packages/
dotnet pack ./PlaygroundShared.Application/PlaygroundShared.Application.csproj -o ./packages/
dotnet pack ./PlaygroundShared.Configurations/PlaygroundShared.Configurations.csproj -o ./packages/
dotnet pack ./PlaygroundShared.Domain/PlaygroundShared.Domain.csproj -o ./packages/
dotnet pack ./PlaygroundShared.Infrastructure.Core/PlaygroundShared.Infrastructure.Core.csproj -o ./packages/
dotnet pack ./PlaygroundShared.Infrastructure.EF/PlaygroundShared.Infrastructure.EF.csproj -o ./packages/
dotnet pack ./PlaygroundShared.Infrastructure.MongoDb/PlaygroundShared.Infrastructure.MongoDb.csproj -o ./packages/
dotnet pack ./PlaygroundShared.IntercontextCommunication/PlaygroundShared.IntercontextCommunication.csproj -o ./packages/
dotnet pack ./PlaygroundShared.IntercontextCommunication.RabbitMq/PlaygroundShared.IntercontextCommunication.RabbitMq.csproj -o ./packages/

dotnet nuget push ./packages/PlaygroundShared.Api.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./packages/PlaygroundShared.Application.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./packages/PlaygroundShared.Configurations.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./packages/PlaygroundShared.Domain.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./packages/PlaygroundShared.Infrastructure.Core.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./packages/PlaygroundShared.Infrastructure.EF.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./packages/PlaygroundShared.Infrastructure.MongoDb.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./packages/PlaygroundShared.IntercontextCommunication.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json
dotnet nuget push ./packages/PlaygroundShared.IntercontextCommunication.RabbitMq.$1.nupkg --api-key $2 --source https://api.nuget.org/v3/index.json

