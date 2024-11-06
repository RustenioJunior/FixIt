FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY FixIt.API/FixIt.API.csproj ./
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libstdc++6 \
        zlib1g
RUN dotnet restore
COPY . .
USER root
RUN dotnet publish FixIt.API/FixIt.API.csproj -c Release -p:PublishSingleFile=true -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./
RUN chmod +x ./FixIt.API.dll
EXPOSE 80
ENTRYPOINT ["dotnet", "FixIt.API.dll"]