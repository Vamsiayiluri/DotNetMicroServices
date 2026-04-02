# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution
COPY ["MicroservicesPOC.sln", "./"]

# ✅ Correct project file names
COPY ["IdentityService/API/IdentityService.API.csproj", "IdentityService/API/"]
COPY ["IdentityService/Application/IdentityService.Application.csproj", "IdentityService/Application/"]
COPY ["IdentityService/Domain/IdentityService.Domain.csproj", "IdentityService/Domain/"]
COPY ["IdentityService/Infrastructure/IdentityService.Infrastructure.csproj", "IdentityService/Infrastructure/"]

# Restore
RUN dotnet restore "IdentityService/API/IdentityService.API.csproj"

# Copy everything
COPY . .

# Build
WORKDIR "/src/IdentityService/API"
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]