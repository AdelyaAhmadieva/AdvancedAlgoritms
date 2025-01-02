FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConsoleApp4/ConsoleApp4.csproj", "ConsoleApp4/"]
RUN dotnet restore "ConsoleApp4/ConsoleApp4.csproj"
COPY . .
WORKDIR "/src/ConsoleApp4"
RUN dotnet build "ConsoleApp4.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp4.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp4.dll"]
