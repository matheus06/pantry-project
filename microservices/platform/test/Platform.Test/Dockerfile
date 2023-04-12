FROM mcr.microsoft.com/dotnet/sdk:7.0

ARG OFFICIAL_NUGET_SOURCE_REPOSITORY=https://api.nuget.org/v3/index.json

WORKDIR /src
COPY ./microservices/platform/src/Platform/Platform.csproj ./Platform/
RUN dotnet restore "Platform/Platform.csproj"  \
    --source ${OFFICIAL_NUGET_SOURCE_REPOSITORY} 
COPY ./microservices/platform/src/ ./

WORKDIR /test/Platform.Tests
COPY ./microservices/platform/test/Platform.Test/ ./
RUN dotnet restore "Platform.Test.csproj" \
    --source ${OFFICIAL_NUGET_SOURCE_REPOSITORY} 

RUN dotnet build "Platform.Test.csproj" --nologo --configuration Release
RUN dotnet test "Platform.Test.csproj" --no-build --nologo --configuration Release --logger:"trx;LogFileName=unit-test-results.xml" --results-directory /results