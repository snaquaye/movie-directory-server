FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MovieDirectory/MovieDirectory.csproj", "MovieDirectory/"]
RUN dotnet restore "src/MovieDirectory/MovieDirectory.csproj"
COPY . .
WORKDIR "/src/MovieDirectory"
RUN dotnet build "MovieDirectory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovieDirectory.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieDirectory.dll"]
