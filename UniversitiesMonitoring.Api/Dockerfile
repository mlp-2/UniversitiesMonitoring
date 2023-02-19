FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["UniversitiesMonitoring.Api/UniversitiesMonitoring.Api.csproj", "UniversitiesMonitoring.Api/"]
RUN dotnet restore "UniversitiesMonitoring.Api/UniversitiesMonitoring.Api.csproj"
COPY . .
WORKDIR "/src/UniversitiesMonitoring.Api"
RUN dotnet build "UniversitiesMonitoring.Api.csproj" -c Release -o /app/build

FROM build AS publish

ARG JWT_SECRET
ARG CONNECTION_STRING

RUN dotnet publish "UniversitiesMonitoring.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "UniversitiesMonitoring.Api.dll"]
