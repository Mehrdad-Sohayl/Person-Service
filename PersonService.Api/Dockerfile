# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_NOLOGO=1
ENV NUGET_FALLBACK_PACKAGES=""
ENV NUGET_RESTORE_MSBUILD_ARGS=

COPY PersonService.Domain/PersonService.Domain.csproj PersonService.Domain/
COPY PersonService.Application/PersonService.Application.csproj PersonService.Application/
COPY PersonService.Contracts/PersonService.Contracts.csproj PersonService.Contracts/
COPY PersonService.Infrastructure/PersonService.Infrastructure.csproj PersonService.Infrastructure/
COPY PersonService.Api/PersonService.Api.csproj PersonService.Api/
COPY NuGet.config .
RUN dotnet restore PersonService.Api/PersonService.Api.csproj

COPY PersonService.Domain/ PersonService.Domain/
COPY PersonService.Application/ PersonService.Application/
COPY PersonService.Contracts/ PersonService.Contracts/
COPY PersonService.Infrastructure/ PersonService.Infrastructure/
COPY PersonService.Api/ PersonService.Api/
RUN dotnet publish PersonService.Api/PersonService.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5001
ENTRYPOINT ["dotnet", "PersonService.Api.dll"]
