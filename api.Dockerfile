#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api_Producer/Api_Producer.csproj", "Api_Producer/"]
COPY ["BuisnessLayer/BuisnessLayer.csproj", "BuisnessLayer/"]
COPY ["DatabaseLayer/DatabaseLayer.csproj", "DatabaseLayer/"]
COPY ["Services/Services.csproj", "Services/"]
RUN dotnet restore "./Api_Producer/./Api_Producer.csproj"
COPY . .
WORKDIR "/src/Api_Producer"
RUN dotnet build "./Api_Producer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api_Producer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api_Producer.dll"]