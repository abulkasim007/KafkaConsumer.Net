# syntax=docker/dockerfile:1

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview-noble-aot AS build
WORKDIR /src

COPY . .

RUN --mount=type=cache,target=/root/.nuget \
    dotnet publish -r linux-x64 KafkaConsumer/KafkaConsumer.csproj -c Release -o /app  && chmod +x /app/KafkaConsumer

# Final image: no runtime needed, it's native
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-preview-noble-chiseled
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["./KafkaConsumer"]