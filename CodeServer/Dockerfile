#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CodeServer/CodeServer.csproj", "CodeServer/"]
COPY ["CodeServer.Business/CodeServer.Business.csproj", "CodeServer.Business/"]
COPY ["CodeServer.Data/CodeServer.Data.csproj", "CodeServer.Data/"]
COPY ["CodeServer.Core/CodeServer.Core.csproj", "CodeServer.Core/"]
RUN dotnet restore "CodeServer/CodeServer.csproj"
COPY . .
WORKDIR "/src/CodeServer"
RUN dotnet build "CodeServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeServer.dll"]