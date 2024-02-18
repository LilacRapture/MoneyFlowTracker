FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /usr/src/app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /usr/src/app
COPY --from=build-env /usr/src/app/out .

EXPOSE 80
EXPOSE 443
EXPOSE 10000
ENTRYPOINT ["dotnet", "MoneyFlowTracker.Api.dll"]