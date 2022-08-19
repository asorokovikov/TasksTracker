FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .

EXPOSE 6000
ENV ASPNETCORE_URLS=http://+:6000

ENTRYPOINT ["/bin/bash", "-c", "dotnet restore && dotnet run --project TasksTracker.Api"]