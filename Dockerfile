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

# Copy the certificates into the container
COPY ./dev.crt /App/certs/dev.crt
COPY ./dev.key /App/certs/dev.key

# Set environment variables to use the certificate (optional)
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/App/certs/dev.crt
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/App/certs/dev.key
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password123

ENTRYPOINT ["dotnet", "BadgerClan.Client.dll"]