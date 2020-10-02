#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WordFinder/WordFinder.csproj", "WordFinder/"]
COPY ["DBEntities/DBEntities.csproj", "DBEntities/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "WordFinder/WordFinder.csproj"
COPY . .
WORKDIR "/src/WordFinder"
RUN dotnet build "WordFinder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WordFinder.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WordFinder.dll"]