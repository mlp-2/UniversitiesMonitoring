FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UniversitiesMonitoring.Module/UniversitiesMonitoring.Module.csproj", "UniversitiesMonitoring.Module/"]
RUN dotnet restore "UniversitiesMonitoring.Module/UniversitiesMonitoring.Module.csproj"
COPY . .
WORKDIR "/src/UniversitiesMonitoring.Module"
RUN dotnet build "UniversitiesMonitoring.Module.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniversitiesMonitoring.Module.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniversitiesMonitoring.Module.dll"]
