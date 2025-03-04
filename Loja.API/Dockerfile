# Etapa base para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar arquivos .csproj corretamente
COPY Loja.API/Loja.API.csproj Loja.API/
COPY Loja.Application/Loja.Application.csproj Loja.Application/
COPY Loja.Domain/Loja.Domain.csproj Loja.Domain/
COPY Loja.Infra.Data/Loja.Infra.Data.csproj Loja.Infra.Data/
COPY Loja.Infra.Ioc/Loja.Infra.Ioc.csproj Loja.Infra.Ioc/

# Restaurar as dependências
RUN dotnet restore Loja.API/Loja.API.csproj

# Copiar todo o código-fonte
COPY . .

# Definir a variável para evitar erro de permissão
ENV DOTNET_EnableDiagnostics=0

# Compilar a aplicação
RUN mkdir -p /app/build && chmod -R 777 /app/build
RUN dotnet build Loja.API/Loja.API.csproj -c $BUILD_CONFIGURATION --no-self-contained -o /app/build

# Publicar a aplicação para produção
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Loja.API/Loja.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Criar a imagem final de produção
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Loja.API.dll"]
