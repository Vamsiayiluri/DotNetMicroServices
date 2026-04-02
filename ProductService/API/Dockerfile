# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY ["MicroservicesPOC.sln", "./"]

# Copy all project files
COPY ["ProductService/API/ProductService.API.csproj", "ProductService/API/"]
COPY ["ProductService/Application/ProductService.Application.csproj", "ProductService/Application/"]
COPY ["ProductService/Domain/ProductService.Domain.csproj", "ProductService/Domain/"]
COPY ["ProductService/Infrastructure/ProductService.Infrastructure.csproj", "ProductService/Infrastructure/"]

# Restore dependencies
RUN dotnet restore "ProductService/API/ProductService.API.csproj"

# Copy everything
COPY . .

# Build
WORKDIR "/src/ProductService/API"
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductService.API.dll"]