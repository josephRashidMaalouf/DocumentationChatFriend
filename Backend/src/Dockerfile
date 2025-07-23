# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all source code 
COPY . .

# Restore and publish the main project
WORKDIR /src/DocumentationChatFriend.Backend.Api
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Set environment to live
ENV ASPNETCORE_ENVIRONMENT=Docker

# Expose the required port
EXPOSE 5143
ENV ASPNETCORE_URLS=http://+:5143

ENTRYPOINT ["dotnet", "DocumentationChatFriend.Backend.Api.dll"]