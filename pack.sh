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