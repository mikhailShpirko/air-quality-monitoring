FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Monitoring/Monitoring.Service/Monitoring.Service.csproj", "Monitoring.Service/"]
COPY ["src/Monitoring/Monitoring.Domain/Monitoring.Domain.csproj", "Monitoring.Domain/"]
COPY ["src/Monitoring/Monitoring.Infrastructure/Monitoring.Infrastructure.csproj", "Monitoring.Infrastructure/"]
RUN dotnet restore "Monitoring.Service/Monitoring.Service.csproj"
COPY ["src/Monitoring/Monitoring.Service/", "Monitoring.Service"]
COPY ["src/Monitoring/Monitoring.Domain/", "Monitoring.Domain"]
COPY ["src/Monitoring/Monitoring.Infrastructure/", "Monitoring.Infrastructure"]
WORKDIR "Monitoring.Service"
RUN dotnet build "Monitoring.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Monitoring.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Monitoring.Service.dll"]