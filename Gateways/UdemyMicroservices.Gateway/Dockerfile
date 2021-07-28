FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build

WORKDIR /app

EXPOSE 80

COPY Gateways/UdemyMicroservices.Gateway/*csproj Gateways/UdemyMicroservices.Gateway/

RUN dotnet restore Gateways/UdemyMicroservices.Gateway/*csproj

COPY . .

RUN dotnet publish Gateways/UdemyMicroservices.Gateway/*csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "UdemyMicroservices.Gateway.dll" ]