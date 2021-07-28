FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build

WORKDIR /app

EXPOSE 80

COPY Shared/UdemyMicroservices.Shared/*.csproj Shared/UdemyMicroservices.Shared/

COPY Services/FakePayment/UdemyMicroservices.FakePayment/*csproj Services/FakePayment/UdemyMicroservices.FakePayment/

RUN dotnet restore Services/FakePayment/UdemyMicroservices.FakePayment/*csproj

COPY . .

RUN dotnet publish Services/FakePayment/UdemyMicroservices.FakePayment/*csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "UdemyMicroservices.FakePayment.dll" ]