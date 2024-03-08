# SPDX-License-Identifier: MIT
# Copyright: 2023 Econolite Systems, Inc.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV SolutionDir /src
WORKDIR /src
COPY . .
# Generate model files
RUN dotnet new tool-manifest
RUN dotnet tool install Mapster.Tool
RUN dotnet build ./Models.PavementCondition/ -c Release -o /app/build
WORKDIR "/src/Simulator.Logging.Producer/"
RUN dotnet build "Simulator.PavementCondition.Logging.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Simulator.PavementCondition.Logging.Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simulator.PavementCondition.Logging.Producer.dll"]
