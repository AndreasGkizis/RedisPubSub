# Stage 1: Build the application
# Use the .NET SDK image to build the app.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container.
WORKDIR /src

# Copy the entire backend project folder into the container.
# This ensures all source files and project files are present for the build.
COPY . backend

# Set the working directory to the project folder.
WORKDIR /src/backend

# Publish the application for the final image.
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Create the final runtime image
# Use the lightweight .NET Runtime image.
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final

# Set the working directory for the final app.
WORKDIR /app

# Copy the published artifacts from the 'build' stage.
# This ensures the final image is as small as possible, containing only the runtime and your compiled app.
COPY --from=build /app/publish .

# Define the entry point for the container.
ENTRYPOINT ["dotnet", "backend.dll"]
