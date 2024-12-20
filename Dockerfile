# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy everything to the container
COPY . .

# Restore dependencies
RUN dotnet restore "backend.csproj" --disable-parallel

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef --version 8.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# Publish the application
RUN dotnet publish "backend.csproj" -c release -o /app --no-restore

# Migrations Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrations
WORKDIR /app

# Copy the published application
COPY --from=build /app ./
COPY . .

# Run the database update
RUN dotnet ef migrations add AddNewFields --project /source/backend
RUN dotnet ef database update --project /source/backend

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Set environment variables
ENV JWT_ISSUER=http://localhost:5203
ENV JWT_AUDIENCE=http://localhost:5203

# Copy the published application from the build stage
COPY --from=build /app ./

# Expose the port
EXPOSE 6000 7002

# Run the application
ENTRYPOINT ["dotnet", "backend.dll"]