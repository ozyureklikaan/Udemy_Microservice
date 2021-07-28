FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build

WORKDIR /app

EXPOSE 80

COPY Shared/UdemyMicroservices.Shared/*.csproj Shared/UdemyMicroservices.Shared/

COPY Services/Discount/UdemyMicroservice.Discount/*csproj Services/Discount/UdemyMicroservice.Discount/

RUN dotnet restore Services/Discount/UdemyMicroservice.Discount/*csproj

COPY . .

RUN dotnet publish Services/Discount/UdemyMicroservice.Discount/*csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "UdemyMicroservice.Discount.dll" ]