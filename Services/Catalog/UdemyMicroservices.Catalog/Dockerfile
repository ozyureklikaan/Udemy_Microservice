FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build

WORKDIR /app

EXPOSE 80

COPY Shared/UdemyMicroservices.Shared/*.csproj Shared/UdemyMicroservices.Shared/

COPY Services/Catalog/UdemyMicroservices.Catalog/*csproj Services/Catalog/UdemyMicroservices.Catalog/

RUN dotnet restore Services/Catalog/UdemyMicroservices.Catalog/*csproj

COPY . .

RUN dotnet publish Services/Catalog/UdemyMicroservices.Catalog/*csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "UdemyMicroservices.Catalog.dll" ]