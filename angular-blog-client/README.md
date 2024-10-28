# Blog Post Management Project

This project was developed with meticulous attention to detail and a focus on performance and user experience. The application allows for managing blog posts, including creating, updating, deleting, and filtering by specific categories or authors. During development, I made an effort to apply best practices and maintain a well-organized and optimized code.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Project Installation](#project-installation)
3. [Database Configuration](#database-configuration)
4. [Useful Commands](#useful-commands)
5. [Performance Considerations](#performance-considerations)

---

### Prerequisites

Before you begin, make sure you have the following tools installed on your system:

- **Node.js** (version 14 or higher)
- **Angular CLI** (version 16 or higher)
- **.NET Core SDK** (if you're using a .NET-based backend for the API and database migrations)
- **SQL Server** or your database management system preferred

### Installing the Project

Follow these steps to set up the development environment for your Angular application:

1. Clone the repository:

```bash
git clone <REPOSITORY-URL>
cd <PROJECT-NAME>
```

Install the Angular dependencies:

```bash
npm install

```
### Start the application:

```bash
ng serve
The application will be available at http://localhost:4200/.
```
### Setting Up the Database
#### Open the appsettings.Development.json file and locate the "ConnectionStrings" section.

Replace it with the following connection string, adjusting the Server value to match the name of your SQL Server instance:

```bash
"ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1,1435; Database=BlogBDV1; User Id=sa; Password=Security2024*; TrustServerCertificate=True;"
}
```
Replace YOUR-SERVER-NAME with the name of your SQL Server instance.

#### To set up the database and apply migrations, use the following Entity Framework command:

```bash
dotnet ef database update
```
This command will apply all migrations and prepare the database with the schema needed for the project.