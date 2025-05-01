# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore ".\PickNPlay.csproj" --disable-parallel
RUN dotnet publish ".\PickNPlay.csproj" -c Release -o /app --no-restore

# serve stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal 
WORKDIR /app 
COPY --from=build /app ./

EXPOSE 5001

ENTRYPOINT ["dotnet", "PickNPlay.dll"]