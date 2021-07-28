FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build

WORKDIR /app

EXPOSE 80

COPY Shared/UdemyMicroservices.Shared/*.csproj Shared/UdemyMicroservices.Shared/

COPY Client/UdemyMicroservices.Web/*.csproj Client/UdemyMicroservices.Web/

RUN dotnet restore Client/UdemyMicroservices.Web/*.csproj

COPY . .

RUN dotnet publish Client/UdemyMicroservices.Web/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "UdemyMicroservices.Web.dll" ]