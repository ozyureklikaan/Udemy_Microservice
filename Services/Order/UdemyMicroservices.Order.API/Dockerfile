FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build

WORKDIR /app

EXPOSE 80

COPY Shared/UdemyMicroservices.Shared/*.csproj Shared/UdemyMicroservices.Shared/

COPY Services/Order/UdemyMicroservices.Order.API/*csproj Services/Order/UdemyMicroservices.Order.API/

COPY Services/Order/UdemyMicroservices.Order.Application/*csproj Services/Order/UdemyMicroservices.Order.Application/

COPY Services/Order/UdemyMicroservices.Order.Domain/*csproj Services/Order/UdemyMicroservices.Order.Domain/

COPY Services/Order/UdemyMicroservices.Order.Domain.Core/*csproj Services/Order/UdemyMicroservices.Order.Domain.Core/

COPY Services/Order/UdemyMicroservices.Order.Infrastructure/*csproj Services/Order/UdemyMicroservices.Order.Infrastructure/

RUN dotnet restore Services/Order/UdemyMicroservices.Order.API/*csproj

COPY . .

RUN dotnet publish Services/Order/UdemyMicroservices.Order.API/*csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "UdemyMicroservices.Order.API.dll" ]