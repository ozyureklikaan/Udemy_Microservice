FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build

WORKDIR /app

EXPOSE 80

COPY Shared/UdemyMicroservices.Shared/*.csproj Shared/UdemyMicroservices.Shared/

COPY Services/Basket/UdemyMicroservices.Basket/*csproj Services/Basket/UdemyMicroservices.Basket/

RUN dotnet restore Services/Basket/UdemyMicroservices.Basket/*csproj

COPY . .

RUN dotnet publish Services/Basket/UdemyMicroservices.Basket/*csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "UdemyMicroservices.Basket.dll" ]