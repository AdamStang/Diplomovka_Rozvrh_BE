#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
VOLUME ${APPDATA}/Microsoft/UserSecrets:/root/.microsoft/usersecrets:ro
VOLUME ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:443;http://+:80
WORKDIR /app
EXPOSE 80
EXPOSE 443

#ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["backend.csproj", "backend/"]
RUN dotnet restore "backend/backend.csproj"
COPY . ./backend
WORKDIR "/src/backend"
RUN dotnet build "backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "backend.dll"]