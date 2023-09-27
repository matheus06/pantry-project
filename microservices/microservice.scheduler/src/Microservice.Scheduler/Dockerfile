FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src
COPY ./microservices/microservice.scheduler/src/Microservice.Scheduler/ ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM build-env AS unit-tests
WORKDIR /results
WORKDIR /test
COPY ./microservices/microservice.scheduler/src/Microservice.Scheduler.Tests/ ./Microservice.Scheduler.Tests/
RUN dotnet restore "Microservice.Scheduler.Tests/Microservice.Scheduler.Tests.csproj" -s https://api.nuget.org/v3/index.json
COPY ./test/Microservice.Scheduler.Tests/ ./Microservice.Scheduler.Tests/
WORKDIR /test/Microservice.Scheduler.Tests
RUN dotnet test Microservice.Scheduler.Tests.csproj --nologo -c Release --logger:"trx;LogFileName=unit-test-results.xml" -r /results

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /src
COPY --from=build-env /src/out .
ENTRYPOINT ["dotnet", "Microservice.Scheduler.dll"]