# This example uses .NET 6.0.  For other versions, see https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

# Build your application
COPY ./src /src/

WORKDIR /src/dotnet-newrelic-log-forward
RUN dotnet publish -c Release -o /app dotnet-newrelic-log-forward.csproj

# The runtime tag version should match the SDK tag version
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

# Install the dependency packages
RUN apt-get update && \
    apt-get install -y --no-install-recommends ca-certificates tzdata curl bc && \
    rm -rf /var/lib/apt/lists/* 

WORKDIR /app

RUN curl --proto '=https' --tlsv1.2 -sSf https://sh.vector.dev | bash -s -- -y

COPY --from=base /app .
COPY src/dotnet-newrelic-log-forward/vector.toml /etc/vector/vector.toml

#CMD ["dotnet", "dotnet-newrelic-log-forward.dll"]
CMD ["/root/.vector/bin/vector"]