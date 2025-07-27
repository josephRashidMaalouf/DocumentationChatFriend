# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all source code 
COPY . .

# Restore and publish the main project
WORKDIR /src/DocumentationChatFriend.DiscordClient.App
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish


# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0

WORKDIR /app
COPY --from=build /app/publish .

ENV BackendClientUri=http://documentation-chat-friend-backend:5143/

ENTRYPOINT ["dotnet", "DocumentationChatFriend.DiscordClient.App.dll"]