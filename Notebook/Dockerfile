FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV LANG=C.UTF-8

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Notebook/Notebook.csproj", "Notebook/"]
COPY ["Notebook.Client/Notebook.Client.csproj", "Notebook.Client/"]
COPY ["Notebook.Shared/Notebook.Shared.csproj", "Notebook.Shared/"]
RUN dotnet restore "./Notebook/Notebook.csproj"
COPY . .
WORKDIR "/src/Notebook"
RUN dotnet build "./Notebook.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Notebook.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notebook.dll"]