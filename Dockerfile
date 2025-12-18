FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["PortfolioApi/Portfolio.Api.csproj", "PortfolioApi/"]
COPY ["Portfolio.Application/Portfolio.Application.csproj", "Portfolio.Application/"]
COPY ["Portfolio.Domain/Portfolio.Domain.csproj", "Portfolio.Domain/"]
COPY ["Portfolio.Infrastructure/Portfolio.Infrastructure.csproj", "Portfolio.Infrastructure/"]

RUN dotnet restore "PortfolioApi/Portfolio.Api.csproj"
COPY . .

WORKDIR "/src/PortfolioApi"
RUN dotnet build "Portfolio.Api.csproj" -c Release -o /app/build
RUN dotnet publish "Portfolio.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]