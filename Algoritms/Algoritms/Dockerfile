FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Algoritms/Algoritms.csproj", "Algoritms/"]
RUN dotnet restore "Algoritms/Algoritms.csproj"
COPY . .
WORKDIR "/src/Algoritms"
RUN dotnet build "Algoritms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Algoritms.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Algoritms.dll"]
