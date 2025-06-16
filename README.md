


<div align="center">
  <img src="./assets/monadicsharp_logo.png" alt="MonadicSharp Logo" width="300"/>
</div>

<div align="center">

```
███╗   ███╗ ██████╗ ███╗   ██╗ █████╗ ██████╗ ██╗ ██████╗
████╗ ████║██╔═══██╗████╗  ██║██╔══██╗██╔══██╗██║██╔════╝
██╔████╔██║██║   ██║██╔██╗ ██║███████║██║  ██║██║██║     
██║╚██╔╝██║██║   ██║██║╚██╗██║██╔══██║██║  ██║██║██║     
██║ ╚═╝ ██║╚██████╔╝██║ ╚████║██║  ██║██████╔╝██║╚██████╗
╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═╝╚═════╝ ╚═╝ ╚═════╝
                    SHARP
```

</div>



# MonadicPilot

A pilot fullstack application showcasing **MonadicSharp** functional programming capabilities. Built with C# and ASP.NET Core backend, and Angular frontend, demonstrating real-world implementation of monadic patterns and functional programming principles.

## 🚀 MonadicSharp Features Demonstrated

This project showcases the power of **MonadicSharp** library through practical implementations:

- **Option<T> Patterns**: Safe null handling without traditional null checks
- **Result<T> Patterns**: Railway-oriented programming for error handling
- **Maybe Monad**: Elegant handling of optional values
- **Functional Composition**: Chaining operations with monadic bind operations
- **Domain-Driven Design**: Clean architecture with functional programming principles
- **Railway-Oriented Programming**: Error handling that flows naturally through the application

### Key MonadicSharp Integrations

- **Customer Domain**: Demonstrates `Option<T>` for safe customer lookups
- **Validation Layer**: Uses `Result<T>` for comprehensive validation handling  
- **Service Layer**: Monadic composition for business logic flow
- **Repository Pattern**: Functional approach to data access
- **Error Handling**: Comprehensive error management using Result patterns

## Project Structure

```
MonadicPilot
├── assets                 # Project assets and logos
├── backend                # Backend application with MonadicSharp
│   ├── API                # Web API layer
│   │   └── Controllers    # API controllers
│   ├── Application        # Application layer (CQRS)
│   │   └── Customer       # Customer commands and queries
│   ├── Controllers        # Legacy controllers
│   ├── Domain             # Domain layer with monadic patterns
│   │   ├── Billing        # Billing domain with MonadicSharp
│   │   ├── Customer       # Customer domain implementation
│   │   ├── Inventory      # Inventory management
│   │   ├── Sales          # Sales domain
│   │   └── Shared         # Shared domain components
│   ├── Infrastructure     # Infrastructure and MonadicSharp extensions
│   │   ├── MonadicSharpExtensions.cs
│   │   └── Persistence    # Data persistence layer
│   ├── Presentation       # Presentation layer
│   ├── Services           # Service interfaces
│   ├── Program.cs         # Entry point for the backend application
│   ├── appsettings.json   # Configuration settings for the backend
│   └── backend.csproj     # Project file for the backend
├── backend.Tests          # Comprehensive unit tests for the backend
│   └── Domain             # Domain-specific tests
│       ├── Billing        # Billing domain tests
│       ├── Customer       # Customer domain tests
│       ├── Inventory      # Inventory tests
│       ├── Sales          # Sales domain tests
│       └── Shared         # Shared component tests
├── frontend               # Frontend application
│   ├── src                # Source files for the Angular app
│   ├── angular.json       # Angular project configuration
│   ├── package.json       # npm configuration for the frontend
│   └── tsconfig.json      # TypeScript configuration
└── MyFullstackApp.sln     # Solution file for the entire project
```

## Backend

The backend is built using ASP.NET Core and demonstrates **MonadicSharp** patterns throughout the application architecture. It showcases:

### Core Components
- **Customer Domain**: Full implementation of DDD with monadic patterns
- **Weather API**: Sample REST endpoints with functional error handling
- **Repository Pattern**: Functional approach to data access using `Option<T>` and `Result<T>`
- **Service Layer**: Business logic composition with monadic bind operations
- **Validation**: Comprehensive validation using `Result<T>` patterns

### MonadicSharp Integration Examples
- **Safe Data Access**: `Option<Customer>` for nullable database queries
- **Error Propagation**: `Result<T>` for operation outcomes with detailed error information
- **Functional Composition**: Chaining operations without traditional exception handling
- **Railway-Oriented Programming**: Clean error flow through the application pipeline

### Setup Instructions

1. Navigate to the `backend` directory.
2. Run the following command to restore dependencies:
   ```
   dotnet restore
   ```
3. Start the backend application:
   ```
   dotnet run
   ```

## Frontend

The frontend is developed using Angular and communicates with the backend API to display weather information. It includes:

- **WeatherComponent**: Displays weather data fetched from the backend.
- **WeatherService**: Handles HTTP requests to the backend API.

### Setup Instructions

1. Navigate to the `frontend` directory.
2. Install the necessary dependencies:
   ```
   npm install
   ```
3. Start the Angular application:
   ```
   ng serve
   ```

## Usage

Once both the backend and frontend applications are running, you can access the frontend at `http://localhost:4200` and the backend API at `http://localhost:5000`.

## Contributing

Feel free to submit issues or pull requests for any improvements or features you would like to see in this project.