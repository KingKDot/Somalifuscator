ARG LAUNCHING_FROM_VS
ARG FINAL_BASE_IMAGE=${LAUNCHING_FROM_VS:+aotdebug}

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SomalifuscatorV3/SomalifuscatorV3.csproj", "SomalifuscatorV3/"]
RUN dotnet restore "./SomalifuscatorV3/SomalifuscatorV3.csproj"
COPY . .
WORKDIR "/src/SomalifuscatorV3"
RUN dotnet build "./SomalifuscatorV3.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SomalifuscatorV3.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS aotdebug
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    gdb
USER app

FROM ${FINAL_BASE_IMAGE:-mcr.microsoft.com/dotnet/runtime-deps:9.0} AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./SomalifuscatorV3"]
