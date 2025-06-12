# My Fullstack App

This project is a fullstack application that consists of a backend built with C# and ASP.NET Core, and a frontend developed using Angular.

## Project Structure

```
my-fullstack-app
├── backend                # Backend application
│   ├── Controllers        # Contains API controllers
│   ├── Models             # Contains data models
│   ├── Services           # Contains service interfaces
│   ├── Program.cs         # Entry point for the backend application
│   ├── appsettings.json   # Configuration settings for the backend
│   └── backend.csproj     # Project file for the backend
├── frontend               # Frontend application
│   ├── src                # Source files for the Angular app
│   ├── angular.json       # Angular project configuration
│   ├── package.json       # npm configuration for the frontend
│   └── tsconfig.json      # TypeScript configuration
└── MyFullstackApp.sln     # Solution file for the entire project
```

## Backend

The backend is built using ASP.NET Core and provides a RESTful API for the frontend to interact with. It includes:

- **WeatherController**: Handles HTTP requests related to weather forecasts.
- **WeatherForecast**: Represents the structure of weather data.
- **IWeatherService**: Interface for fetching weather data.

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