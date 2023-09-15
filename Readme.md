# ASP.NET 6 Driver CRUD Application

This ASP.NET Core 6 application is designed to perform CRUD (Create, Read, Update, Delete) operations for the `Driver` class. The purpose is to manage driver information efficiently.

## Features

- **Get Driver by ID**: Retrieve driver information by specifying the driver's ID.
- **Delete Driver by ID**: Delete a driver based on their ID.
- **Create Driver**: Add a new driver to the system.
- **Update Driver**: Modify existing driver details.
- **Get All Drivers**: Retrieve a list of all drivers.

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

## How to Use the Project

1. **Run the Project**: Navigate to the base directory of the application and execute the follwoing commands
`cd Backend`. 
`dotnet run --urls=http://localhost:5000`. you can replace '5000' with whatever valid port to run the app on

2- to open swagger , you can use the url where app listening to , here url=http://localhost:5000/swagger

2. **Run Tests**:
   - Change the directory to `BackTests.csproj` by executing the follwoing commcands
   
   `cd Backend.Tests`.
   `dotnet build`.     this is to build the test project and ensures there no erors before testing
   `dotnet test`.     this is to run the project tests

Feel free to explore the API documentation through Swagger, which provides an interactive interface to interact with the API endpoints.

For any inquiries or issues, please contact [Your Name](mailto:mohamed2511995@gmail.com).
