# Task Manager Frontend

This is the frontend application for the Task Manager project, built with Vue 3, Vite, Vuetify, Tailwind CSS, and Pinia.

## Technologies Used

- **Vue 3**: Progressive JavaScript framework
- **Vite**: Fast build tool and development server
- **Vuetify**: Material Design component framework
- **Tailwind CSS**: Utility-first CSS framework
- **Pinia**: State management library
- **Vue Router**: Official router for Vue.js
- **Axios**: HTTP client for API requests

## Project Structure

```
src/
├── components/          # Reusable Vue components
├── views/              # Page components
├── stores/             # Pinia stores for state management
├── services/           # API service modules
├── router/             # Vue Router configuration
├── App.vue             # Root component
├── main.js             # Application entry point
└── style.css           # Global styles
```

## Getting Started

### Prerequisites

- Node.js 16+ and npm

### Installation

1. Install dependencies:
```bash
npm install
```

2. Start the development server:
```bash
npm run dev
```

3. Open your browser and navigate to `http://localhost:3000`

### Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint
- `npm run format` - Format code with Prettier

## Features

- **Authentication**: Login and registration
- **Dashboard**: Overview of projects and tasks
- **Project Management**: Create, edit, and delete projects
- **Task Management**: Create, edit, complete, and delete tasks
- **Responsive Design**: Works on desktop and mobile devices
- **Modern UI**: Material Design components with Vuetify
- **State Management**: Centralized state with Pinia
- **API Integration**: RESTful API communication

## Environment Variables

Create a `.env` file in the root directory:

```env
VITE_API_BASE_URL=http://localhost:5000/api
```

## API Integration

The frontend communicates with the ASP.NET Core backend API. The API client is configured in `src/services/apiClient.js` and includes:

- Automatic JWT token attachment
- Request/response interceptors
- Error handling
- Base URL configuration

## State Management

The application uses Pinia for state management with the following stores:

- **User Store**: Authentication and user data
- **Project Store**: Project management
- **Task Store**: Task management  
- **App Store**: Global app state (loading, notifications)

## Styling

The application combines Vuetify's Material Design components with Tailwind CSS utilities for maximum flexibility and rapid development.
