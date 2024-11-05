FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pitang.OrderBook.Metrics.Worker/Pitang.OrderBook.Metrics.Worker.csproj", "Pitang.OrderBook.Metrics.Worker/"]
RUN dotnet restore "./Pitang.OrderBook.Metrics.Worker/./Pitang.OrderBook.Metrics.Worker.csproj"
COPY . .
WORKDIR "/src/Pitang.OrderBook.Metrics.Worker"
RUN dotnet build "./Pitang.OrderBook.Metrics.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pitang.OrderBook.Metrics.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pitang.OrderBook.Metrics.Worker.dll"]