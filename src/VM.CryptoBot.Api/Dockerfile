#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VM.CryptoBot.Api/VM.CryptoBot.Api.csproj", "VM.CryptoBot.Api/"]
COPY ["VM.CryptoBot.AppServices/VM.CryptoBot.AppServices.csproj", "VM.CryptoBot.AppServices/"]
COPY ["VM.CryptoBot.Workers/VM.CryptoBot.Workers.csproj", "VM.CryptoBot.Workers/"]
COPY ["VM.CryptoBot.Infra/VM.CryptoBot.Infra.csproj", "VM.CryptoBot.Infra/"]
COPY ["VM.CryptoBot.Domain/VM.CryptoBot.Domain.csproj", "VM.CryptoBot.Domain/"]
RUN dotnet restore "./VM.CryptoBot.Api/VM.CryptoBot.Api.csproj"
COPY . .
WORKDIR "/src/VM.CryptoBot.Api"
RUN dotnet build "./VM.CryptoBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VM.CryptoBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppApi=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VM.CryptoBot.Api.dll"]