# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj", "CDR-Bank.IdentityServer/"]
COPY ["CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj", "CDR-Bank.DataAccess/"]
COPY ["CDR-Bank.IndentityServer.Services/CDR-Bank.IndentityServer.Services.csproj", "CDR-Bank.IndentityServer.Services/"]
COPY ["CDR-Bank.Libs.Identity.Contracts/CDR-Bank.Libs.Identity.Contracts.csproj", "CDR-Bank.Libs.Identity.Contracts/"]
COPY ["CDR-Bank.Libs.API/CDR-Bank.Libs.API.csproj", "CDR-Bank.Libs.API/"]
RUN dotnet restore "./CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj"
COPY . .
WORKDIR "/src/CDR-Bank.IdentityServer"
RUN dotnet build "./CDR-Bank.IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CDR-Bank.IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final

ENV ConnectionStrings__IdentityDb="Server=localhost;Port=3306;Database=IdentityDb;User=root;Password=rootpassword;SslMode=Preferred;"
ENV ApiSettings__SecretKey="meGeNdIcklaryBErSKineYKNoWLiNEuSnESCoNAreCIonsABLe"

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CDR-Bank.IdentityServer.dll"]