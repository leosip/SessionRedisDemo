# Use a imagem do SDK do .NET 6 para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos do projeto e restaura as dependências
COPY *.csproj .
RUN dotnet restore

# Copia o restante dos arquivos e constrói a aplicação
COPY . .
RUN dotnet publish -c Release -o out

# Use a imagem do runtime do .NET 6 para executar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expõe a porta 80 para o tráfego HTTP
EXPOSE 80

# Define o comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "SessionDemo.dll"]
