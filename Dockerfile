#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["outputconfigsvc/outputconfigsvc.csproj", "outputconfigsvc/"]
RUN dotnet restore "outputconfigsvc/outputconfigsvc.csproj"
COPY . .
WORKDIR "/src/outputconfigsvc"
RUN dotnet build "outputconfigsvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "outputconfigsvc.csproj" -c Release -o /app/publish --self-contained --runtime linux-x64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/bin/bash", "entrypoint.sh"]