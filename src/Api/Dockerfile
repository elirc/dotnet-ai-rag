FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY NetAiOps.sln ./
COPY src ./src
COPY tests ./tests
RUN dotnet restore NetAiOps.sln
RUN dotnet publish src/Api/NetAiOps.Api.csproj -c Release -o /app/publish --no-restore

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NetAiOps.Api.dll"]
