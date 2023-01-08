FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UniversitiesMonitoring.NotifyService/UniversitiesMonitoring.NotifyService.csproj", "UniversitiesMonitoring.NotifyService/"]
RUN dotnet restore "UniversitiesMonitoring.NotifyService/UniversitiesMonitoring.NotifyService.csproj"
COPY . .
WORKDIR "/src/UniversitiesMonitoring.NotifyService"
RUN dotnet build "UniversitiesMonitoring.NotifyService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniversitiesMonitoring.NotifyService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniversitiesMonitoring.NotifyService.dll"]
