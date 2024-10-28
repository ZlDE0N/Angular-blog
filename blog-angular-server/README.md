Here is the detailed README to install and configure the backend at `./blog-server-angular/`.

---

# Backend Installation Guide for Blog Server Angular

This guide provides the steps to set up the backend for the Blog Server Angular project.

## Prerequisites

- **Docker**: Ensure Docker is installed. If not, download Docker Desktop for Windows from this [link](https://desktop.docker.com/win/main/amd64/Docker%20Desktop%20Installer.exe?utm_source=docker&utm_medium=webreferral&utm_campaign=dd-smartbutton&utm_location=module) and install it.
- **Visual Studio**: Required to open and manage the solution.

## Step-by-Step Instructions

### 1. Clone the Repository

Clone the project repository into the directory `./blog-server-angular/`:

```bash
git clone https://github.com/ZlDE0N/Angular-Blog ./blog-server-angular/
```

### 2. Build and Run SQL Server Docker Image

In the root directory of the project (`./blog-server-angular/`), you will find a `Dockerfile`. Use the following command to build and run the SQL Server container:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Security2024*" -p 1434:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

> The container will start SQL Server on port `1434`. This instance will be accessible for database operations required by the backend.

### 3. Open the Project Solution in Visual Studio

In the root directory `./blog-server-angular/`, locate and open the solution file:

```
BackendBlogServicesApi.sln
```

This will launch Visual Studio with the project solution.

### 4. Restore Dependencies and Run Database Migrations

1. In Visual Studio, go to **Tools > NuGet Package Manager > Package Manager Console**.
2. In the console, navigate to the API project directory:

    ```bash
    cd .\BackendBlogServicesApi
    ```

3. Run database migrations to set up the initial database schema:

    ```bash
    Update-Database
    ```

### 5. Build and Publish the Project

To publish the project in Release mode, use the following command:

```bash
dotnet publish -c Release
```

This will build the project and place the published output in `bin/Release/net8.0/publish/`.

### 6. Run the Published API

To start the API in production mode, run:

```bash
dotnet bin/Release/net8.0/publish/BackendBlogServicesApi.dll --urls "https://localhost:5001"
```

### 7. Access the API Documentation

Open a browser and navigate to:

```
https://localhost:5001
```

This will open Swagger, where you can view the REST API documentation and test the endpoints.

---

You have now set up and launched the backend for Blog Server Angular!