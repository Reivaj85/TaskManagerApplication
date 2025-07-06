# Task Manager Backend API

A Task Manager application backend built using .NET 8 Web API following Clean Architecture principles.

## Architecture

This project follows Clean Architecture with clear separation of concerns across four layers:

- **Domain Layer** (`TaskManager.Domain`): Core business entities and repository interfaces
- **Application Layer** (`TaskManager.Application`): Use cases, services, and DTOs
- **Infrastructure Layer** (`TaskManager.Infrastructure`): Data access implementations and external services
- **Presentation Layer** (`TaskManager.API`): Web API controllers and HTTP handling

## Project Structure

```
TaskManagerApplication/
├── TaskManager.Domain/           # Domain entities and interfaces
├── TaskManager.Application/      # Use cases and application services
├── TaskManager.Infrastructure/   # Data access and external services
├── TaskManager.API/             # Web API controllers
├── TaskManager.Tests/           # Unit and integration tests
└── TaskManagerApplicationBackend.sln
```

## Technologies Used

- **.NET 8**: Latest version of .NET framework
- **ASP.NET Core Web API**: For building RESTful APIs
- **SQLite**: Lightweight database for development
- **JWT Authentication**: For secure user authentication
- **BCrypt**: For password hashing
- **xUnit**: For unit testing
- **Moq**: For mocking in tests

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio Code or Visual Studio

### Build and Run

1. **Build the solution:**
   ```bash
   dotnet build
   ```

2. **Run the API:**
   ```bash
   dotnet run --project TaskManager.API
   ```

3. **Run tests:**
   ```bash
   dotnet test
   ```

### VS Code Tasks

This project includes VS Code tasks for common operations:

- **Build**: `Cmd+Shift+P` → "Tasks: Run Task" → "build"
- **Run**: `Cmd+Shift+P` → "Tasks: Run Task" → "run"
- **Test**: `Cmd+Shift+P` → "Tasks: Run Task" → "test"

## Features

- User authentication and authorization
- Task management (CRUD operations)
- Project management (CRUD operations)
- Default personal project for each user
- Unit of Work pattern for transaction management
- Repository pattern for data access
- Clean Architecture implementation
- Comprehensive testing setup
- SQLite database with automatic schema creation
- Value objects with validation
- Result pattern for error handling

## API Endpoints

The API will include the following endpoints:

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration

### Projects
- `GET /api/projects` - Get user's projects
- `POST /api/projects` - Create new project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project

### Tasks
- `GET /api/projects/{projectId}/tasks` - Get tasks for a project
- `POST /api/projects/{projectId}/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task
- `PATCH /api/tasks/{id}/complete` - Mark task as complete

## Development

### Adding New Features

1. Start with the **Domain layer** - define entities and interfaces
2. Create **Use Cases** in the Application layer
3. Implement **Infrastructure** services (repositories, external services)
4. Add **Controllers** in the API layer
5. Write **Tests** for all layers

### Database

The application uses SQLite for development. The database will be created automatically when you first run the application with the following schema:

- **Users**: User accounts with hashed passwords
- **Projects**: User projects with default project support
- **Tasks**: Tasks belonging to projects with completion status

The Infrastructure layer includes:
- Repository pattern implementations for each entity
- Unit of Work pattern for transaction management
- Database connection factory for dependency injection
- Automatic database schema initialization

## Testing

The project includes comprehensive testing:

- **Unit Tests**: Test business logic in isolation
- **Integration Tests**: Test data access and API endpoints
- **Test Patterns**: Arrange-Act-Assert (AAA) pattern

Run tests with:
```bash
dotnet test
```

## Contributing

1. Follow Clean Architecture principles
2. Write tests for new features
3. Use the provided coding guidelines in `.github/copilot-instructions.md`
4. Ensure all tests pass before submitting changes

## License

This project is for educational and interview purposes.
