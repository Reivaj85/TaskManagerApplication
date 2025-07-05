# TaskManagerApplication

Note: This project began as a proof of concept for a technical interview at the Ballast Lane company.

Task Manager is a simple web application that allows authenticated users to create, read, update, and delete tasks organized into projects (lists). Every user has a default personal project, and can create additional projects. The backend is a RESTful API built with ASP.NET Core and the frontend uses Vue 3. The solution follows Clean Architecture principles and Test-Driven Development (TDD). Data access is implemented at a low level (without Entity Framework, Dapper, or Mediator).
