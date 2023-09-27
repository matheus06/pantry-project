FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src
COPY ./microservices/microservice.recipemanager/src/Microservice.RecipeManager/ ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM build-env AS unit-tests
WORKDIR /results
WORKDIR /test
COPY ./microservices/microservice.recipemanager/src/Microservice.RecipeManager.Tests/ ./Microservice.RecipeManager.Tests/
RUN dotnet restore "Microservice.RecipeManager.Tests/Microservice.RecipeManager.Tests.csproj" -s https://api.nuget.org/v3/index.json
COPY ./test/Microservice.RecipeManager.Tests/ ./Microservice.RecipeManager.Tests/
WORKDIR /test/Microservice.RecipeManager.Tests
RUN dotnet test Microservice.RecipeManager.Tests.csproj --nologo -c Release --logger:"trx;LogFileName=unit-test-results.xml" -r /results

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /src
COPY --from=build-env /src/out .
ENTRYPOINT ["dotnet", "Microservice.RecipeManager.dll"]