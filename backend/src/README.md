# Task Manager Backend

A .NET 8 Web API implementing a task management system following Clean Architecture principles, SOLID design patterns, and Domain-Driven Design (DDD).

## ✅ Architecture Overview

The solution is organized into four main projects following Clean Architecture:

### 🏗️ Domain Layer (`TaskManager.Domain`)
- **Entities**: Core business objects (User, Project, TaskItem)
- **Value Objects**: UserName, Password, PasswordHash, ProjectName, TaskTitle, TaskDescription
- **Interfaces**: Repository contracts (IUserRepository, IProjectRepository, ITaskRepository, IUnitOfWork)
- **Common**: Result pattern for error handling

### 🚀 Application Layer (`TaskManager.Application`)
- **Use Cases**: Business logic services (AuthenticationService, ProjectService, TaskService, UserService)
- **DTOs**: Data transfer objects for API communication
- **Mappings**: Entity to DTO conversion extensions
- **Interfaces**: External service contracts (ITokenService)

### 🗄️ Infrastructure Layer (`TaskManager.Infrastructure`)
- **Data Access**: Repository implementations using raw SQL
- **Unit of Work**: Transaction management pattern
- **Database**: SQLite with connection factory pattern
- **Database Initialization**: Automated schema creation

### 🌐 API Layer (`TaskManager.API`)
- **Controllers**: RESTful API endpoints with JWT authentication
- **Services**: JWT token generation
- **Middleware**: Authentication, authorization, CORS, Swagger
- **Configuration**: JWT settings, database connection strings

## ✅ Features Implemented

### Authentication & Security
- [x] User registration and login
- [x] JWT token authentication
- [x] Password hashing with BCrypt
- [x] Protected API endpoints
- [x] CORS support

### Project Management
- [x] Create, read, update, delete projects
- [x] Default "Personal" project for new users
- [x] Project task count tracking
- [x] User-specific project isolation

### Task Management
- [x] Create, read, update, delete tasks
- [x] Tasks belong to projects
- [x] Task completion status
- [x] Task descriptions and titles

### API Documentation
- [x] Swagger/OpenAPI documentation
- [x] Interactive API testing interface
- [x] JWT Bearer token authentication in Swagger

## ✅ Technical Implementation

### Clean Architecture & SOLID Principles
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Entities and value objects are extensible
- **Liskov Substitution**: Repository interfaces are properly implemented
- **Interface Segregation**: Focused, cohesive interfaces
- **Dependency Inversion**: Domain depends on abstractions, not implementations

### Design Patterns
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management
- **Factory Pattern**: Database connection creation
- **Result Pattern**: Error handling without exceptions
- **Value Objects**: Encapsulated domain concepts
- **Entity Pattern**: Business objects with identity

### Database Design
- **Raw SQL**: No ORM dependency, full control over queries
- **Transactions**: ACID compliance with Unit of Work
- **Schema Management**: Automated database initialization
- **SQLite**: Lightweight, file-based database for development

## ✅ API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login

### Projects
- `GET /api/projects` - Get user projects
- `GET /api/projects/{id}` - Get specific project
- `POST /api/projects` - Create project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project

### Tasks
- `GET /api/tasks/project/{projectId}` - Get project tasks
- `GET /api/tasks/{id}` - Get specific task
- `POST /api/tasks` - Create task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

### Users
- `GET /api/users/profile` - Get user profile
- `PUT /api/users/profile` - Update user profile

## ✅ Testing

Comprehensive test suite covering all layers:

### Test Coverage
- **77 passing tests** with 0 failures
- **Domain Layer**: Entity behavior, value object validation, business rules
- **Infrastructure Layer**: Repository operations, database integration, Unit of Work
- **Application Layer**: DTO mappings, service logic validation

### Test Categories
- **Unit Tests**: Domain entities and value objects
- **Integration Tests**: Repository and database operations
- **Mapping Tests**: DTO conversion validation

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQLite (included with .NET)

### Running the Application

1. **Build the solution:**
   ```bash
   dotnet build
   ```

2. **Run tests:**
   ```bash
   dotnet test
   ```

3. **Start the API:**
   ```bash
   dotnet run --project TaskManager.API
   ```

4. **Access Swagger documentation:**
   ```
   http://localhost:5207/swagger
   ```

### Sample API Usage

1. **Register a user:**
   ```bash
   curl -X POST "http://localhost:5207/api/auth/register" \
     -H "Content-Type: application/json" \
     -d '{"username": "testuser", "password": "Test123!"}'
   ```

2. **Login:**
   ```bash
   curl -X POST "http://localhost:5207/api/auth/login" \
     -H "Content-Type: application/json" \
     -d '{"username": "testuser", "password": "Test123!"}'
   ```

3. **Get projects (requires JWT token):**
   ```bash
   curl -X GET "http://localhost:5207/api/projects" \
     -H "Authorization: Bearer YOUR_JWT_TOKEN"
   ```

## ✅ Configuration

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!",
    "Issuer": "TaskManagerAPI",
    "Audience": "TaskManagerClient",
    "ExpirationHours": 24
  }
}
```

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=taskmanager-dev.db"
  }
}
```

## ✅ Project Status

**Status**: ✅ **COMPLETED**

### Completed Features
- [x] Clean Architecture implementation
- [x] Domain entities with business logic
- [x] Value objects with validation
- [x] Repository pattern with Unit of Work
- [x] JWT authentication and authorization
- [x] Complete RESTful API
- [x] Database schema initialization
- [x] Comprehensive test suite (77 tests passing)
- [x] API documentation with Swagger
- [x] Error handling with Result pattern
- [x] SOLID principles adherence
- [x] Full CRUD operations for all entities

### Production Readiness
The application includes:
- Proper error handling and validation
- Security best practices (JWT, password hashing)
- Automated database schema management
- Comprehensive test coverage
- API documentation
- Clean separation of concerns
- Maintainable and extensible codebase

This implementation demonstrates enterprise-level .NET development practices with Clean Architecture, proper testing, and production-ready features.
