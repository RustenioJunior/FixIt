FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /app
EXPOSE 44308
EXPOSE 5235

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Authentication.dll"]
