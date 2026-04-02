# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY ["MicroservicesPOC.sln", "./"]

# Copy all project files
COPY ["OrderService/API/OrderService.API.csproj", "OrderService/API/"]
COPY ["OrderService/Application/OrderService.Application.csproj", "OrderService/Application/"]
COPY ["OrderService/Domain/OrderService.Domain.csproj", "OrderService/Domain/"]
COPY ["OrderService/Infrastructure/OrderService.Infrastructure.csproj", "OrderService/Infrastructure/"]

# Restore dependencies
RUN dotnet restore "OrderService/API/OrderService.API.csproj"

# Copy everything
COPY . .

# Build
WORKDIR "/src/OrderService/API"
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]