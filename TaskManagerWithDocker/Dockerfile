#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskManagerWithDocker/TaskManagerWithDocker.csproj", "TaskManagerWithDocker/"]
RUN dotnet restore "./TaskManagerWithDocker/TaskManagerWithDocker.csproj"
COPY . .
WORKDIR "/src/TaskManagerWithDocker"
RUN dotnet build "./TaskManagerWithDocker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskManagerWithDocker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef database update

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerWithDocker.dll"]

