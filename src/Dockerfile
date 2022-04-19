FROM mcr.microsoft.com/dotnet/sdk:6.0
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
WORKDIR /app
COPY . .
ENTRYPOINT dotnet watch run --urls=http://+:3030