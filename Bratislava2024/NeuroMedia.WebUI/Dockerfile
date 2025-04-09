FROM nginx AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY NeuroMedia.WebUI/NeuroMedia.WebUI.csproj .
COPY NeuroMedia.Application.*csproj NeuroMedia.Application/
COPY NeuroMedia.Domain.*csproj NeuroMedia.Domain/
RUN dotnet restore "NeuroMedia.WebUI.csproj"

COPY NeuroMedia.WebUI/ NeuroMedia.WebUI/
COPY NeuroMedia.Application/ NeuroMedia.Application/
COPY NeuroMedia.Domain NeuroMedia.Domain/
WORKDIR "/src/NeuroMedia.WebUI"
RUN dotnet build "NeuroMedia.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NeuroMedia.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM  base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY  NeuroMedia.WebUI/nginx.conf /etc/nginx/nginx.conf

#FROM nginx AS base
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#
#WORKDIR /src
#COPY NeuroMedia.WebUI/NeuroMedia.WebUI.csproj .
#RUN dotnet restore "NeuroMedia.WebUI.csproj"
#
#COPY . .
#WORKDIR "/src/NeuroMedia.WebUI"
#RUN dotnet build "NeuroMedia.WebUI.csproj" -c Release -o /app/build
#
#RUN dotnet restore NeuroMedia.WebUI/NeuroMedia.WebUI.csproj
#
## copy and build app and libraries
#COPY NeuroMedia.WebUI/ NeuroMedia.WebUI/
#COPY NeuroMedia.Application/ NeuroMedia.Application/
#COPY NeuroMedia.Domain/ NeuroMedia.Domain/
#COPY NeuroMedia.sln NeuroMedia.sln
#
#WORKDIR /src/NeuroMedia.WebUI
#RUN dotnet build -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "NeuroMedia.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM  base AS final
#WORKDIR /usr/share/nginx/html
#COPY --from=publish /app/publish/wwwroot .
#COPY  NeuroMedia.WebUI/nginx.conf /etc/nginx/nginx.conf