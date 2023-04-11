FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src
COPY ./microservices/microservice.pantrymanager/src/Microservice.PantryManager/ ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM build-env AS unit-tests
WORKDIR /results
WORKDIR /test
COPY ./microservices/microservice.pantrymanager/src/Microservice.PantryManager.Tests/ ./Microservice.PantryManager.Tests/
RUN dotnet restore "Microservice.PantryManager.Tests/Microservice.PantryManager.Tests.csproj" -s https://api.nuget.org/v3/index.json
COPY ./test/Microservice.PantryManager.Tests/ ./Microservice.PantryManager.Tests/
WORKDIR /test/Microservice.PantryManager.Tests
RUN dotnet test Microservice.PantryManager.Tests.csproj --nologo -c Release --logger:"trx;LogFileName=unit-test-results.xml" -r /results

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /src
COPY --from=build-env /src/out .
ENTRYPOINT ["dotnet", "Microservice.PantryManager.dll"]