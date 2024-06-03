#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["sync_api_gateway/sync_api_gateway.csproj", "sync_api_gateway/"]
RUN dotnet restore "sync_api_gateway/sync_api_gateway.csproj"
COPY . .
WORKDIR "/src/sync_api_gateway"
RUN dotnet build "sync_api_gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "sync_api_gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sync_api_gateway.dll"]