#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 8000
ENV ASPNETCORE_URLS="http://+:8000"

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["NinKode.WebApi/NinKode.WebApi.csproj", "NinKode.WebApi/"]
RUN dotnet restore "NinKode.WebApi/NinKode.WebApi.csproj"
COPY . .
WORKDIR "/src/NinKode.WebApi"
RUN dotnet build "NinKode.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NinKode.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
RUN groupadd -r --gid 1007 dockerrunner && useradd -r -g dockerrunner dockerrunner
WORKDIR /app
COPY --from=publish /app/publish .
USER dockerrunner
ENTRYPOINT ["dotnet", "NinKode.WebApi.dll"]
