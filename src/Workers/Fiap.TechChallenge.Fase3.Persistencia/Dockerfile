FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Workers/Fiap.TechChallenge.Fase3.Persistencia/Fiap.TechChallenge.Fase3.Persistencia.csproj", "/src/Workers/Fiap.TechChallenge.Fase3.Persistencia/"]
COPY ["src/Workers/Fiap.TechChallenge.Fase3.RabbitMQ/Fiap.TechChallenge.Fase3.RabbitMQ.csproj", "/src/Workers/Fiap.TechChallenge.Fase3.RabbitMQ/"]
COPY ["src/Core/Fiap.TechChallenge.Fase1.SharedKernel/Fiap.TechChallenge.Fase1.SharedKernel.csproj", "/src/Core/Fiap.TechChallenge.Fase1.SharedKernel/"]
COPY ["src/Services/Fiap.TechChallenge.Fase1.IoC/Fiap.TechChallenge.Fase1.IoC.csproj", "/src/Services/Fiap.TechChallenge.Fase1.IoC/"]
COPY ["src/Services/Fiap.TechChallenge.Fase1.Data/Fiap.TechChallenge.Fase1.Data.csproj", "/src/Services/Fiap.TechChallenge.Fase1.Data/"]
COPY ["src/Services/Fiap.TechChallenge.Fase1.Dominio/Fiap.TechChallenge.Fase1.Dominio.csproj", "/src/Services/Fiap.TechChallenge.Fase1.Dominio/"]
COPY ["src/Services/Fiap.TechChallenge.Fase1.Infraestructure/Fiap.TechChallenge.Fase1.Infraestructure.csproj", "/src/Services/Fiap.TechChallenge.Fase1.Infraestructure/"]

RUN dotnet restore "/src/Workers/Fiap.TechChallenge.Fase3.Persistencia/Fiap.TechChallenge.Fase3.Persistencia.csproj"

COPY . .
WORKDIR "src/Workers/Fiap.TechChallenge.Fase3.Persistencia"
RUN dotnet build "Fiap.TechChallenge.Fase3.Persistencia.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Fiap.TechChallenge.Fase3.Persistencia.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fiap.TechChallenge.Fase3.Persistencia.dll"]
