FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
VOLUME /app/data

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StockWebAPI/StockWebAPI.csproj", "StockWebAPI/"]
COPY ["StockBusinessLayer/StockBusinessLayer.csproj", "StockBusinessLayer/"]
COPY ["DiffCalculator/DiffCalculator.csproj", "DiffCalculator/"]
COPY ["FileLoader/FileLoader.csproj", "FileLoader/"]
COPY ["GenericBusinessLayer/GenericBusinessLayer.csproj", "GenericBusinessLayer/"]
COPY ["GenericInfrastructure/GenericInfrastructure.csproj", "GenericInfrastructure/"]
COPY ["GenericDataAccessLayer/GenericDataAccessLayer.csproj", "GenericDataAccessLayer/"]
COPY ["StockInfrastructure/StockInfrastructure.csproj", "StockInfrastructure/"]
COPY ["StockDataAccessLayer/StockDataAccessLayer.csproj", "StockDataAccessLayer/"]
COPY ["StockAPI/StockAPI.csproj", "StockAPI/"]
COPY ["StockDAL.SQLite.Migrations/StockDAL.SQLite.Migrations.csproj", "StockDAL.SQLite.Migrations/"]
RUN dotnet restore "StockWebAPI/StockWebAPI.csproj"
COPY . .
WORKDIR "/src/StockWebAPI"
RUN dotnet build "StockWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StockWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockWebAPI.dll"]
