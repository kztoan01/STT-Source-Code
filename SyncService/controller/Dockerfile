FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-jammy-amd64 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-amd64 AS build
WORKDIR /src
RUN mkdir api core data repository service
COPY ["controller/controller.csproj", "/src/api"]
COPY ["core/core.csproj", "/src/core"]
COPY ["data/data.csproj", "/src/data"]
COPY ["repository/repository.csproj", "/src/repository"]
COPY ["service/service.csproj", "/src/service"]
RUN dotnet restore "/src/api/controller.csproj"
COPY ["controller/", "/src/api"]
COPY ["core/", "/src/core"]
COPY ["data/", "/src/data"]
COPY ["repository/", "/src/repository"]
COPY ["service/", "/src/service"]

WORKDIR /src/api
RUN dotnet build "controller.csproj" --configuration Release --runtime linux-x64 -o ./build

FROM build as publish
WORKDIR /src/api
RUN dotnet publish "controller.csproj" -c Release -o /app/out

RUN apt-get update && apt-get install -y locales \
    && locale-gen en_US.UTF-8 \
    && update-locale LANG=en_US.UTF-8

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LANG en_US.UTF-8

FROM base AS final
COPY --from=publish /app/out .

RUN apt-get update && apt-get install -y iputils-ping

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENTRYPOINT [ "dotnet", "controller.dll"]
