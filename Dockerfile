FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OVB.Demos.Eschody.WebApi/OVB.Demos.Eschody.WebApi.csproj", "src/OVB.Demos.Eschody.WebApi/"]
COPY ["libs/OVB.Demos.Eschody.Libraries.NotificationContext/OVB.Demos.Eschody.Libraries.NotificationContext.csproj", "libs/OVB.Demos.Eschody.Libraries.NotificationContext/"]
COPY ["libs/OVB.Demos.Eschody.Libraries.Observability/OVB.Demos.Eschody.Libraries.Observability.csproj", "libs/OVB.Demos.Eschody.Libraries.Observability/"]
COPY ["libs/OVB.Demos.Eschody.Libraries.ValueObjects/OVB.Demos.Eschody.Libraries.ValueObjects.csproj", "libs/OVB.Demos.Eschody.Libraries.ValueObjects/"]
COPY ["libs/OVB.Demos.Eschody.Libraries.ProcessResultContext/OVB.Demos.Eschody.Libraries.ProcessResultContext.csproj", "libs/OVB.Demos.Eschody.Libraries.ProcessResultContext/"]
COPY ["src/OVB.Demos.Eschody.Application/OVB.Demos.Eschody.Application.csproj", "src/OVB.Demos.Eschody.Application/"]
COPY ["src/OVB.Demos.Eschody.Domain/OVB.Demos.Eschody.Domain.csproj", "src/OVB.Demos.Eschody.Domain/"]
COPY ["src/OVB.Demos.Eschody.Infrascructure/OVB.Demos.Eschody.Infrascructure.csproj", "src/OVB.Demos.Eschody.Infrascructure/"]
RUN dotnet restore "./src/OVB.Demos.Eschody.WebApi/./OVB.Demos.Eschody.WebApi.csproj"
COPY . .
WORKDIR "/src/src/OVB.Demos.Eschody.WebApi"
RUN dotnet build "./OVB.Demos.Eschody.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OVB.Demos.Eschody.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OVB.Demos.Eschody.WebApi.dll"]