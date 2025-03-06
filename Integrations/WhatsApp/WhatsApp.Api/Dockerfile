# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Integrations/WhatsApp/WhatsApp.Api/WhatsApp.Api.csproj", "Integrations/WhatsApp/WhatsApp.Api/"]
COPY ["Integrations/WhatsApp/WhatsApp.Core/WhatsApp.Core.csproj", "Integrations/WhatsApp/WhatsApp.Core/"]
COPY ["Shared/AdaptiveKitCore/AdaptiveKitCore.csproj", "Shared/AdaptiveKitCore/"]
COPY ["Shared/CrossCutting/CrossCutting.csproj", "Shared/CrossCutting/"]
COPY ["Integrations/WhatsApp/WhatsApp.Data/WhatsApp.Data.csproj", "Integrations/WhatsApp/WhatsApp.Data/"]
RUN dotnet restore "./Integrations/WhatsApp/WhatsApp.Api/WhatsApp.Api.csproj"
COPY . .
WORKDIR "/src/Integrations/WhatsApp/WhatsApp.Api"
RUN dotnet build "./WhatsApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WhatsApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WhatsApp.Api.dll"]