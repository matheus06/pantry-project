ARG OFFICIAL_NUGET_SOURCE_REPOSITORY=https://api.nuget.org/v3/index.json
ARG GITHUB_SOURCE_REPOSITORY=https://nuget.pkg.github.com/matheus06/index.json
ARG GITHUB_PACKAGES_USERNAME=matheus06
ARG GITHUB_PACKAGES_TOKEN=PLACEHOLDER

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG OFFICIAL_NUGET_SOURCE_REPOSITORY
ARG GITHUB_SOURCE_REPOSITORY
ARG GITHUB_PACKAGES_USERNAME
ARG GITHUB_PACKAGES_TOKEN
RUN dotnet nuget add source --username ${GITHUB_PACKAGES_USERNAME} --password ${GITHUB_PACKAGES_TOKEN} --store-password-in-clear-text --name github ${GITHUB_SOURCE_REPOSITORY}

WORKDIR /src
COPY ./aspire/aspire.ServiceDefaults/ ./../../../aspire/aspire.ServiceDefaults
COPY ./platform/src/Platform/ ./../../../platform/src/Platform
COPY ./microservice.productmanager/src/Microservice.ProductManager/ ./Microservice.ProductManager
RUN dotnet restore "Microservice.ProductManager/Microservice.ProductManager.csproj"  --runtime linux-x64 \
    --source ${OFFICIAL_NUGET_SOURCE_REPOSITORY} \
    --source ${GITHUB_SOURCE_REPOSITORY}
RUN dotnet publish "Microservice.ProductManager/Microservice.ProductManager.csproj" -c Release -o out


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /src
COPY --from=build-env /src/out ./
ENTRYPOINT ["dotnet", "Microservice.ProductManager.dll"]