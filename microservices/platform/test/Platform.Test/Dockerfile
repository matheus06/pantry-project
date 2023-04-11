FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /src
COPY ./microservices/platform/src/Platform/Platform.csproj ./Platform/
RUN dotnet restore "Platform/Platform.csproj"
COPY ./microservices/platform/src/ ./


WORKDIR /test/Platform.Tests
COPY ./microservices/platform/test/Platform.Test/ ./
RUN dotnet restore
RUN dotnet build
RUN dotnet test
RUN dotnet test