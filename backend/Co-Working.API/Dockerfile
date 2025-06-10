FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5086

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Co-Working.API/Co-Working.API/Co-Working.API.csproj", "Co-Working.API/"]
COPY ["Co-Working.API/Co-Woring.Application/Co-Woring.Application.csproj", "Co-Woring.Application/"]
COPY ["Co-Working.API/Co-Working.Domain/Co-Working.Core.csproj", "Co-Working.Domain/"]
COPY ["Co-Working.API/Co-Working.Persistence/Co-Working.Persistence.csproj", "Co-Working.Persistence/"]

RUN dotnet restore "Co-Working.API/Co-Working.API.csproj"

COPY . .

WORKDIR "/src/Co-Working.API/Co-Working.API"
RUN dotnet build "Co-Working.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Co-Working.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Co-Working.API.dll"]