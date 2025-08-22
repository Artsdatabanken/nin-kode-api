#!/bin/sh
set -e
service ssh start
exec dotnet NinKode.WebApi.dll