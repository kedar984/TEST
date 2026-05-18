# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SqlHelps.Web/SqlHelps.Web.csproj", "SqlHelps.Web/"]
RUN dotnet restore "SqlHelps.Web/SqlHelps.Web.csproj"
COPY . .
WORKDIR "/src/SqlHelps.Web"
RUN dotnet build "SqlHelps.Web.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "SqlHelps.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SqlHelps.Web.dll"]
