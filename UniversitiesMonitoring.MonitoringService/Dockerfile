FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UniversitiesMonitoring.MonitoringService/UniversitiesMonitoring.MonitoringService.csproj", "UniversitiesMonitoring.MonitoringService/"]
RUN dotnet restore "UniversitiesMonitoring.MonitoringService/UniversitiesMonitoring.MonitoringService.csproj"
COPY . .
WORKDIR "/src/UniversitiesMonitoring.MonitoringService"
RUN dotnet build "UniversitiesMonitoring.MonitoringService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniversitiesMonitoring.MonitoringService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniversitiesMonitoring.MonitoringService.dll"]
