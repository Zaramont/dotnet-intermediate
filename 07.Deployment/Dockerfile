#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["07.Deployment.csproj", "."]
RUN dotnet restore "./07.Deployment.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "07.Deployment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "07.Deployment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "07.Deployment.dll"]