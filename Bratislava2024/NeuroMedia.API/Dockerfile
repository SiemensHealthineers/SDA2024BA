#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
#COPY nuget.config .

# copy csproj and restore as distinct layers
COPY NeuroMedia.API/*.csproj NeuroMedia.API/
COPY NeuroMedia.Application/*.csproj NeuroMedia.Application/
COPY NeuroMedia.Domain/*.csproj NeuroMedia.Domain/
COPY NeuroMedia.Infrastructure/*.csproj NeuroMedia.Infrastructure/
COPY NeuroMedia.Persistence/*.csproj NeuroMedia.Persistence/

RUN dotnet restore NeuroMedia.API/NeuroMedia.API.csproj

# copy and build app and libraries
COPY NeuroMedia.API/ NeuroMedia.API/
COPY NeuroMedia.Application/ NeuroMedia.Application/
COPY NeuroMedia.Domain/ NeuroMedia.Domain/
COPY NeuroMedia.Infrastructure/ NeuroMedia.Infrastructure/
COPY NeuroMedia.Persistence/ NeuroMedia.Persistence/
COPY NeuroMedia.sln NeuroMedia.sln

WORKDIR /src/NeuroMedia.API
RUN dotnet build -c Release -o /app/build

FROM build AS publish

RUN dotnet publish -c Release -o /app/publish

# Use a non-root user for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Create a non-root user and group called "appuser" with UID and GID of 1000
RUN addgroup --gid 1000 appuser && adduser --uid 1000 --ingroup appuser --disabled-password --gecos "" appuser

WORKDIR /app

# Set the ownership of the working directory and its contents to the "appuser"
RUN chown -R appuser:appuser /app

# Switch to the non-root user
USER appuser

COPY --from=publish --chown=appuser:appuser /app/publish .

ENV ASPNETCORE_URLS http://0.0.0.0:5000

EXPOSE 5000
ENTRYPOINT ["dotnet", "NeuroMedia.API.dll"]
