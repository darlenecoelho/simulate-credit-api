# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/SimulateCredit.API/SimulateCredit.API.csproj", "SimulateCredit.API/"]
COPY ["src/SimulateCredit.Application/SimulateCredit.Application.csproj", "SimulateCredit.Application/"]
COPY ["src/SimulateCredit.Infrastructure/SimulateCredit.Infrastructure.csproj", "SimulateCredit.Infrastructure/"]
COPY ["src/SimulateCredit.Domain/SimulateCredit.Domain.csproj", "SimulateCredit.Domain/"]

WORKDIR /src/SimulateCredit.API
RUN dotnet restore

COPY src/ ./ 
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build /p:UseAppHost=false

# Publish
FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final API runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimulateCredit.API.dll"]