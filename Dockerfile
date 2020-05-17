FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 23456
ENV ASPNETCORE_URLS="http://localhost:23456"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["cipher-server.csproj", "./"]
RUN dotnet restore "./cipher-server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "cipher-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cipher-server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cipher-server.dll"]
