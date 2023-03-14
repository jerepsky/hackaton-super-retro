FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HackathonSuperRetro/HackathonSuperRetro.csproj", "HackathonSuperRetro/"]
COPY ["NuGet.Config", "HackathonSuperRetro/"]
RUN dotnet restore "HackathonSuperRetro/HackathonSuperRetro.csproj"
COPY . .
WORKDIR "/src/HackathonSuperRetro"
RUN dotnet build "HackathonSuperRetro.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackathonSuperRetro.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackathonSuperRetro.dll"]
