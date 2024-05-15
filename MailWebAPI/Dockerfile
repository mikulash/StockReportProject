FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
VOLUME /app/data

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MailWebAPI/MailWebAPI.csproj", "MailWebAPI/"]
COPY ["MailBusinessLayer/MailBusinessLayer.csproj", "MailBusinessLayer/"]
COPY ["GenericBusinessLayer/GenericBusinessLayer.csproj", "GenericBusinessLayer/"]
COPY ["GenericInfrastructure/GenericInfrastructure.csproj", "GenericInfrastructure/"]
COPY ["GenericDataAccessLayer/GenericDataAccessLayer.csproj", "GenericDataAccessLayer/"]
COPY ["MailAPI/MailAPI.csproj", "MailAPI/"]
COPY ["MailInfrastructure/MailInfrastructure.csproj", "MailInfrastructure/"]
COPY ["MailDataAccessLayer/MailDataAccessLayer.csproj", "MailDataAccessLayer/"]
COPY ["MailDAL.SQLite.Migrations/MailDAL.SQLite.Migrations.csproj", "MailDAL.SQLite.Migrations/"]
RUN dotnet restore "MailWebAPI/MailWebAPI.csproj"
COPY . .
WORKDIR "/src/MailWebAPI"
RUN dotnet build "MailWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MailWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailWebAPI.dll"]
