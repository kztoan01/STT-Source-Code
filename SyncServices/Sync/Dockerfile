# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-jammy-amd64 AS base
EXPOSE 8080
EXPOSE 80

# Image for build process
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-amd64 AS build
WORKDIR /src
RUN mkdir api 
COPY ["./Sync/Sync.csproj", "/src/api"]
RUN dotnet restore "/src/api/Sync.csproj"
COPY ["./Sync", "/src/api"]

WORKDIR /src/api
RUN dotnet build "Sync.csproj" --configuration Release --runtime linux-x64 -o ./build

# Publish stage
FROM build as publish
RUN dotnet publish -c Release -o /app/out

RUN apt-get update && apt-get install -y locales \
    && locale-gen en_US.UTF-8 \
    && update-locale LANG=en_US.UTF-8

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LANG en_US.UTF-8

# Final stage
FROM base AS final
COPY --from=publish /app/out .

RUN apt-get update && apt-get install -y iputils-ping

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENTRYPOINT [ "dotnet", "Sync.dll" ]