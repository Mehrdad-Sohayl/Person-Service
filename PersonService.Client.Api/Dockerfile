# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_NOLOGO=1
ENV NUGET_FALLBACK_PACKAGES=""
ENV NUGET_RESTORE_MSBUILD_ARGS=

COPY PersonService.Contracts/PersonService.Contracts.csproj PersonService.Contracts/
COPY PersonService.Client.Api/PersonService.Client.Api.csproj PersonService.Client.Api/
COPY NuGet.config .
RUN dotnet restore PersonService.Client.Api/PersonService.Client.Api.csproj

COPY PersonService.Contracts/ PersonService.Contracts/
COPY PersonService.Client.Api/ PersonService.Client.Api/
RUN dotnet publish PersonService.Client.Api/PersonService.Client.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PersonService.Client.Api.dll"]
