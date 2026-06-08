FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Steam.Web.Api/Steam.Web.Api.csproj", "Steam.Web.Api/"]
COPY ["SteamApplication/SteamApplication.csproj", "SteamApplication/"]
COPY ["SteamDomain/SteamDomain.csproj", "SteamDomain/"]
COPY ["SteamInfrastructure/SteamInfrastructure.csproj", "SteamInfrastructure/"]
COPY ["SteamShared/SteamShared.csproj", "SteamShared/"]

RUN dotnet restore "Steam.Web.Api/Steam.Web.Api.csproj"

COPY . .

RUN dotnet publish "Steam.Web.Api/Steam.Web.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "Steam.Web.Api.dll"]
