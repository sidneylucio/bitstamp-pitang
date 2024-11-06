FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["/src/Pitang.OrderBook.Api/Pitang.OrderBook.Api.csproj", "src/Pitang.OrderBook.Api/"]
COPY ["/src/Pitang.OrderBook.Infra.IoC/Pitang.OrderBook.Infra.IoC.csproj", "src/Pitang.OrderBook.Infra.IoC/"]
COPY ["/src/Pitang.OrderBook.Application/Pitang.OrderBook.Application.csproj", "src/Pitang.OrderBook.Application/"]
COPY ["/src/Pitang.OrderBook.Domain/Pitang.OrderBook.Domain.csproj", "src/Pitang.OrderBook.Domain/"]
COPY ["/src/Pitang.OrderBook.Infra.CrossCutting/Pitang.OrderBook.Infra.CrossCutting.csproj", "src/Pitang.OrderBook.Infra.CrossCutting/"]
COPY ["/src/Pitang.OrderBook.Infra.Data/Pitang.OrderBook.Infra.Data.csproj", "src/Pitang.OrderBook.Infra.Data/"]

RUN dotnet restore "./src/Pitang.OrderBook.Api/./Pitang.OrderBook.Api.csproj"
COPY . .

WORKDIR "/src/src/Pitang.OrderBook.Api"
RUN dotnet build "./Pitang.OrderBook.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Pitang.OrderBook.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN ls -la /app

COPY src/Pitang.OrderBook.Api/appsettings.Development.json /app/appsettings.json
RUN if [ "$BUILD_TYPE" = "dev" ]; then COPY ["src/Pitang.OrderBook.Api/appsettings.Development.json", "/app/appsettings.xxz.json" ; fi

ENTRYPOINT ["dotnet", "Pitang.OrderBook.Api.dll"]
