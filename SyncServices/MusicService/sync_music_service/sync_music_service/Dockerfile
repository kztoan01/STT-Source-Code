#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["sync_music_service/sync_music_service.csproj", "sync_music_service/"]
RUN dotnet restore "sync_music_service/sync_music_service.csproj"
COPY . .
WORKDIR "/src/sync_music_service"
RUN dotnet build "sync_music_service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "sync_music_service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sync_music_service.dll"]