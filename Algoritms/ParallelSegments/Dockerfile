FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ParallelSegments/ParallelSegments.csproj", "ParallelSegments/"]
RUN dotnet restore "ParallelSegments/ParallelSegments.csproj"
COPY . .
WORKDIR "/src/ParallelSegments"
RUN dotnet build "ParallelSegments.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParallelSegments.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParallelSegments.dll"]
