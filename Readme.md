# ASP.NET 6 Driver CRUD Application

This ASP.NET Core 6 application is designed to perform CRUD (Create, Read, Update, Delete) operations for the `Driver` class. The purpose is to manage driver information efficiently.

## Features

- **Get Driver by ID**: Retrieve driver information by specifying the driver's ID.
- **Delete Driver by ID**: Delete a driver based on their ID.
- **Create Driver**: Add a new driver to the system.
- **Update Driver**: Modify existing driver details.
- **Get All Drivers**: Retrieve a list of all drivers.

- **Insert 10 Random Names**: For inserting 10 random names
- **Get All Names**: Fetch all the inserted names
- **Get Alphapetized Names By Index**: Get the alphapetized names'spellings by index of name

## Configuration

The application utilizes a `SeederSettings` class to configure seeding behavior. If `EnableSeeding` is set to `true`, the application will seed the database with 10 random driver records during startup. If set to `false`, the application will not perform seeding.

You configure this settings from appsettings.json file

## Technologies Used

- .NET Core 6
- Fluent Validation
- Repository Pattern
- Record Pattern
- xUnit Testing
- Mocking
- In-memory Database for Testing
- SQLite Database for the Application
- Swagger for OpenAPI documentation
- Serilog for logging 


## Environment to run the app (.NET 6 Project)

This guide provides steps to set up and run a .NET 6 project on your machine.

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Step-by-Step Guide

1. **Install .NET 6 SDK:**

   Download and install the .NET 6 SDK from the [official website](https://dotnet.microsoft.com/download/dotnet/6.0).

2. **Verify Installation:**

   Open a terminal and run the command:
   ```bash
   dotnet --version


## How to Use the Project

1- Before running teh app , make sure that you have .net 6 runtime installed on your machine 

2. **Run the Project**: Navigate to the base directory of the application and execute the follwoing commands
`cd Backend`. 
`dotnet run --urls=http://localhost:5000`. you can replace '5000' with whatever valid port to run the app on

3- to open swagger , you can use the url where app listening to , here url=http://localhost:5000/swagger

4. **Run Tests**:
   - Change the directory to `BackTests.csproj` by executing the follwoing commcands
   
   `cd Backend.Tests`.
   `dotnet build`.     this is to build the test project and ensures there no erors before testing
   `dotnet test`.     this is to run the project tests

Feel free to explore the API documentation through Swagger, which provides an interactive interface to interact with the API endpoints.

For any inquiries or issues, please contact [Your Name](mailto:mohamed2511995@gmail.com).
