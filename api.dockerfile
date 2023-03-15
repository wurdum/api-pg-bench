FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-amd64 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ApiPgBench.Web/ApiPgBench.Web.csproj", "src/ApiPgBench.Web/"]
RUN dotnet restore "src/ApiPgBench.Web/ApiPgBench.Web.csproj" -r linux-x64
COPY . .
RUN dotnet build "src/ApiPgBench.Web/ApiPgBench.Web.csproj" -c Release -o /app/build -r linux-x64 --self-contained false --no-restore

FROM build AS publish
RUN dotnet publish "src/ApiPgBench.Web/ApiPgBench.Web.csproj" -c Release -o /app/publish -r linux-x64 --self-contained false --no-restore

FROM base AS final
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT dotnet ApiPgBench.Web.dll
