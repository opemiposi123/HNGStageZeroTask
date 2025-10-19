# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the csproj file first (for caching)
COPY ["HNGStageZeroTask/HNGStageZeroTask.csproj", "HNGStageZeroTask/"]

# Restore dependencies
RUN dotnet restore "HNGStageZeroTask/HNGStageZeroTask.csproj"

# Copy the rest of the source code
COPY . .

# Ensure clean build (delete any cached obj/bin just in case)
RUN rm -rf HNGStageZeroTask/bin HNGStageZeroTask/obj

# Build the project
WORKDIR "/src/HNGStageZeroTask"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HNGStageZeroTask.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HNGStageZeroTask.dll"]
