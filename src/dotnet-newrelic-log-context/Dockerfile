# This example uses .NET 6.0.  For other versions, see https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

# Build your application
COPY ./src /src/

WORKDIR /src/dotnet-newrelic-log-context
RUN dotnet publish -c Release -o /app dotnet-newrelic-log-context.csproj

# The runtime tag version should match the SDK tag version
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

# Install the agent
RUN apt-get update && \
    apt-get install -y wget ca-certificates gnupg --no-install-recommends && \
    echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list && \
    wget -q https://download.newrelic.com/548C16BF.gpg && \
    apt-key add 548C16BF.gpg && \
    apt-get update && \
    apt-get install -y newrelic-dotnet-agent --no-install-recommends && \
    rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
    CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
    NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true \
    NEW_RELIC_APPLICATION_LOGGING_ENABLED=true \
    NEW_RELIC_APPLICATION_LOGGING_FORWARDING_ENABLED=true \
    NEW_RELIC_APPLICATION_LOGGING_FORWARDING_MAX_SAMPLES_STORED=10000 \
    NEW_RELIC_APPLICATION_LOGGING_LOCAL_DECORATING_ENABLED=false \
    NEW_RELIC_APP_NAME=dotnet-newrelic-log-context

WORKDIR /app
COPY --from=base /app .

ENTRYPOINT ["dotnet", "dotnet-newrelic-log-context.dll"]