FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FileStorage/FileStorage.csproj", "FileStorage/"]
COPY ["FileStorage.Application/FileStorage.Application.csproj", "FileStorage.Application/"]
COPY ["FileStorage.Domain/FileStorage.Domain.csproj", "FileStorage.Domain/"]
RUN dotnet restore "FileStorage/FileStorage.csproj"
COPY . .
WORKDIR "/src/FileStorage"
RUN dotnet build "FileStorage.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FileStorage.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/FileStorage && chown $APP_UID /app/FileStorage
ENTRYPOINT ["dotnet", "FileStorage.dll"]