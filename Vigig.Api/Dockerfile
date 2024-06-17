FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV TZ=Asia/Bangkok

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Vigig.Api/Vigig.Api.csproj", "Vigig.Api/"]
RUN dotnet restore "Vigig.Api/Vigig.Api.csproj"
COPY . .
WORKDIR "/src/Vigig.Api"
RUN dotnet build "Vigig.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vigig.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vigig.Api.dll"]
