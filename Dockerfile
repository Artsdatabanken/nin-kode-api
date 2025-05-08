#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
#RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
#RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
EXPOSE 8000 2222
ENV ASPNETCORE_URLS="http://+:8000"

# FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
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

COPY entrypoint.sh ./

# Start and enable SSH
RUN apt-get update \
    && apt-get install -y --no-install-recommends dialog \
    && apt-get install -y --no-install-recommends openssh-server \
    && echo "root:Docker!" | chpasswd \
    && chmod u+x ./entrypoint.sh
COPY sshd_config /etc/ssh/

ENTRYPOINT [ "./entrypoint.sh" ]
