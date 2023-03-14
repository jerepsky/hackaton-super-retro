FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["hackaton-super-retro/hackaton-super-retro.csproj", "hackaton-super-retro/"]
COPY ["NuGet.Config", "hackaton-super-retro/"]
RUN dotnet restore "hackaton-super-retro/hackaton-super-retro.csproj"
COPY . .
WORKDIR "/src/hackaton-super-retro"
RUN dotnet build "hackaton-super-retro.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "hackaton-super-retro.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "hackaton-super-retro.dll"]
