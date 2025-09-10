## ------------------------------------------------------------------------------------------ ##
##   This stage is used when running from VS in fast mode (Default for Debug configuration)   ##
## ------------------------------------------------------------------------------------------ ##

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app

## ------------------------------------------------------------------------------- ##
##   This stage is used to create a tarball of node_modules and the .cproj files   ##
## ------------------------------------------------------------------------------- ##

FROM alpine:latest AS prebuild-tarball
WORKDIR /temp

# -- Project files

COPY ["**/*.csproj","./"]
RUN for file in *.csproj; do \
        dirname="${file%.*}"; \
        mkdir -p "$dirname"; \
        mv "$file" "$dirname/"; \ 
    done

RUN tar -cvf projectfiles.tar *

## --------------------------------------------------- ##
##   This stage is used to build the service project   ##
## --------------------------------------------------- ##

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY --from=prebuild-tarball /temp/projectfiles.tar .
RUN tar -xvf projectfiles.tar && rm projectfiles.tar

RUN dotnet restore "./CZ.Worker.EmailSender/CZ.Worker.EmailSender.csproj"
COPY . .

WORKDIR "/src/CZ.Worker.EmailSender"
RUN dotnet build "./CZ.Worker.EmailSender.csproj" -c $BUILD_CONFIGURATION -o /app/build

## ------------------------------------------------------------------------------------- ##
##   This stage is used to publish the service project to be copied to the final stage   ##
## ------------------------------------------------------------------------------------- ##

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CZ.Worker.EmailSender.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

## ----------------------------------------------------------------------------------------------------------------------------- ##
##   This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)   ##
## ----------------------------------------------------------------------------------------------------------------------------- ##

FROM base AS final

# --  NodeJS Runtime folder

WORKDIR /
COPY --from=publish /app/publish/Scripts ./node-runtime
WORKDIR /node-runtime
USER root
RUN apt-get update && apt-get install -y nodejs npm
RUN npm install
USER app

# --

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CZ.Worker.EmailSender.dll"]