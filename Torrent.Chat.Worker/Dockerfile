#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Torrent.Chat.Worker/Torrent.Chat.Worker.csproj", "Torrent.Chat.Worker/"]
COPY ["Torrent.Chat.Storage/Torrent.Chat.Storage.csproj", "Torrent.Chat.Storage/"]
RUN dotnet restore "./Torrent.Chat.Worker/Torrent.Chat.Worker.csproj"
COPY . .
WORKDIR "/src/Torrent.Chat.Worker"
RUN dotnet build "./Torrent.Chat.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Torrent.Chat.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Torrent.Chat.Worker.dll"]