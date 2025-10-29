#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

# for ci
FROM repos.gbt.gbtad.com:9443/gbt_base/aspnet:3.1 AS base
COPY ./bin/release/netcoreapp3.1/publish/ /app/

# for local
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
#COPY ./GoRegister/bin/release/netcoreapp3.1/publish/ /app/

WORKDIR /app/


EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "GoRegister.dll"]