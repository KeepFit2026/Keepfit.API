FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KeepFit.Backend.API/KeepFit.Backend.API.csproj", "KeepFit.Backend.API/"]
RUN dotnet restore "KeepFit.Backend.API/KeepFit.Backend.API.csproj"
COPY . .
WORKDIR "/src/KeepFit.Backend.API"
RUN dotnet build "./KeepFit.Backend.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KeepFit.Backend.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KeepFit.Backend.API.dll"]
