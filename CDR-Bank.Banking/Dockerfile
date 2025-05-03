FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

RUN apt-get update && \
    apt-get install -y --allow-unauthenticated libc6-dev libgdiplus libx11-dev && \
    apt-get install -y gettext && \
    apt-get install -y moreutils && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["/", "/"]

WORKDIR "/CDR-Bank.Banking"

RUN dotnet restore "CDR-Bank.Banking.csproj"
RUN dotnet build "CDR-Bank.Banking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CDR-Bank.Banking.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

COPY ["/CDR-Bank.Banking/entrypoint.sh", "/app/entrypoint.sh"]

RUN chmod +x ./entrypoint.sh

ENTRYPOINT bash ./entrypoint.sh