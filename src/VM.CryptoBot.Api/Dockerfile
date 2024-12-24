# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Stage for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore "VM.CryptoBot.Api/VM.CryptoBot.Api.csproj"
RUN dotnet build "VM.CryptoBot.Api/VM.CryptoBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage for running unit tests
FROM build AS unit-tests
WORKDIR /src
RUN dotnet restore "VM.CryptoBot.Tests/VM.CryptoBot.Tests.csproj"
RUN dotnet build "VM.CryptoBot.Tests/VM.CryptoBot.Tests.csproj" -c Debug
RUN mkdir -p /src/testresults
RUN dotnet test "VM.CryptoBot.Tests/VM.CryptoBot.Tests.csproj" --no-build --logger:trx --results-directory /src/testresults

# Stage for running integration tests
FROM build AS integration-tests
WORKDIR /src
RUN dotnet restore "VM.CryptoBot.IntegrationTests/VM.CryptoBot.IntegrationTests.csproj"
RUN dotnet build "VM.CryptoBot.IntegrationTests/VM.CryptoBot.IntegrationTests.csproj" -c Debug
RUN mkdir -p /src/testresults
RUN dotnet test "VM.CryptoBot.IntegrationTests/VM.CryptoBot.IntegrationTests.csproj" --no-build --logger:trx --results-directory /src/testresults

# Stage for publishing the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VM.CryptoBot.Api/VM.CryptoBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppApi=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "/app/VM.CryptoBot.Api.dll"]
