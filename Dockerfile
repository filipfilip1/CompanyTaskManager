# Stage 1: Build .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY CompanyTaskManager.Web/CompanyTaskManager.Web.csproj ./CompanyTaskManager.Web/
COPY CompanyTaskManager.Application/CompanyTaskManager.Application.csproj ./CompanyTaskManager.Application/
COPY CompanyTaskManager.Common/CompanyTaskManager.Common.csproj ./CompanyTaskManager.Common/
COPY CompanyTaskManager.Data/CompanyTaskManager.Data.csproj ./CompanyTaskManager.Data/
COPY CompanyTaskManager.UnitTests/CompanyTaskManager.UnitTests.csproj ./CompanyTaskManager.UnitTests/

# Restore dependencies
RUN dotnet restore CompanyTaskManager.Web.sln

# Copy the remaining source files
COPY CompanyTaskManager.Web/ ./CompanyTaskManager.Web/
COPY CompanyTaskManager.Application/ ./CompanyTaskManager.Application/
COPY CompanyTaskManager.Common/ ./CompanyTaskManager.Common/
COPY CompanyTaskManager.Data/ ./CompanyTaskManager.Data/
COPY CompanyTaskManager.UnitTests/ ./CompanyTaskManager.UnitTests/

# Publish the application
RUN dotnet publish CompanyTaskManager.Web/CompanyTaskManager.Web.csproj -c Release -o /app/publish

# Stage 2: Create a minimal runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Set the entry point
ENTRYPOINT ["dotnet", "CompanyTaskManager.Web.dll"]
