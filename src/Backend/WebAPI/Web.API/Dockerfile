FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Backend/WebAPI/Web.API/Web.API.csproj", "src/Backend/WebAPI/Web.API/"]
COPY ["src/Backend/Core/Application/Application.csproj", "src/Backend/Core/Application/"]
COPY ["src/Backend/Shared/Shared/Shared.csproj", "src/Backend/Shared/Shared/"]
COPY ["src/Backend/Core/Domain/Domain.csproj", "src/Backend/Core/Domain/"]
COPY ["src/Backend/Infrastructure/Infrastructure/Infrastructure.csproj", "src/Backend/Infrastructure/Infrastructure/"]
COPY ["src/Backend/Infrastructure/Persistence/Persistence.csproj", "src/Backend/Infrastructure/Persistence/"]
RUN dotnet restore "./src/Backend/WebAPI/Web.API/Web.API.csproj"
COPY . .
WORKDIR "/src/src/Backend/WebAPI/Web.API"
RUN dotnet build "./Web.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


# SSL sertifikalarini kopyala
# COPY ["src/BaseAppSql/Backend/WebAPI/Web.API/ssl", "/app/ssl/"]
COPY ["src/Backend/WebAPI/Web.API/ssl", "/app/ssl/"]


ENTRYPOINT ["dotnet", "Web.API.dll"]