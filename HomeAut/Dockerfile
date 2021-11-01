#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HomeAut/HomeAut.csproj", "HomeAut/"]
COPY ["HomeAut.YeeLight/HomeAut.YeeLight.csproj", "HomeAut.YeeLight/"]
COPY ["HomeAuth.Common/HomeAuth.Common.csproj", "HomeAuth.Common/"]
RUN dotnet restore "HomeAut/HomeAut.csproj"
COPY . .
WORKDIR "/src/HomeAut"
RUN dotnet build "HomeAut.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeAut.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeAut.dll"]