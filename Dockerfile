# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files separately to leverage Docker layer caching
COPY RapidReachNET.sln ./
COPY RapidReachNET.csproj ./

# Restore dependencies
RUN dotnet restore RapidReachNET.csproj

# Copy the rest of the source code
COPY . .

# Publish
RUN dotnet publish RapidReachNET.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Optional: document an exposed port (Render injects PORT; Program.cs binds to it)
EXPOSE 8080

# Environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Run
ENTRYPOINT ["dotnet", "RapidReachNET.dll"]


