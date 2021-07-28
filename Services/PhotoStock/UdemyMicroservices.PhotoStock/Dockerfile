FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build

WORKDIR /app

EXPOSE 80

COPY Shared/UdemyMicroservices.Shared/*.csproj Shared/UdemyMicroservices.Shared/

COPY Services/PhotoStock/UdemyMicroservices.PhotoStock/*csproj Services/PhotoStock/UdemyMicroservices.PhotoStock/

RUN dotnet restore Services/PhotoStock/UdemyMicroservices.PhotoStock/*csproj

COPY . .

RUN dotnet publish Services/PhotoStock/UdemyMicroservices.PhotoStock/*csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "UdemyMicroservices.PhotoStock.dll" ]