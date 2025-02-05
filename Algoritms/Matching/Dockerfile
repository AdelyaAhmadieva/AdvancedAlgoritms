FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Matching/Matching.csproj", "Matching/"]
RUN dotnet restore "Matching/Matching.csproj"
COPY . .
WORKDIR "/src/Matching"
RUN dotnet build "Matching.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Matching.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Matching.dll"]
