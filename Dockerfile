# Prepare the projects
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App
COPY . ./
RUN dotnet restore "BadgerClan.Client/BadgerClan.Client.csproj"
RUN dotnet publish "BadgerClan.Client/BadgerClan.Client.csproj" -c Release -o publish

# Build the image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App
COPY --from=build /App/publish .
ENTRYPOINT ["dotnet", "BadgerClan.Client.dll"]