# Changelog

All notable changes to CompanyTaskManager project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

## [0.1.0] - 2025-06-30

### Added
- **Authentication & Authorization System**
  - User registration with role selection (Employee/Manager)
  - Identity integration with ASP.NET Core
  - Role-based access control
  - Admin approval system for role requests

- **Team Management**
  - Team creation and management by Managers
  - Employee assignment to teams
  - Team-based task and project organization

- **Task Management**
  - Standalone task creation and assignment
  - Task status tracking (Pending, In Progress, Completed, etc.)
  - Task approval workflow
  - Due date management with overdue indicators

- **Project Management**
  - Multi-task project creation
  - Project leader assignment
  - Project-wide status tracking
  - Project approval upon completion of all tasks

- **Notification System**
  - In-app notifications for task assignments
  - Notification for status changes and approvals
  - Manager notifications for task submissions
  - Comprehensive notification management UI

- **Calendar Integration**
  - Interactive calendar view using FullCalendar.js
  - Task and project timeline visualization
  - Due date tracking and visual indicators

- **User Management**
  - User profile management
  - Account activation/deactivation by administrators
  - Role request system with admin approval

- **Technical Features**
  - Clean Architecture with layered separation
  - Entity Framework Core with SQL Server
  - AutoMapper for object mapping
  - Responsive UI with Bootstrap
  - Docker containerization support
  - Database seeding for initial users and data

### Technical Implementation
- **Web Layer**: MVC controllers, Razor views, ViewComponents
- **Application Layer**: Business services, ViewModels, AutoMapper profiles
- **Data Layer**: EF Core entities, DbContext, Fluent API configurations
- **Common Layer**: Shared constants and enums

### Testing
- Integration tests for notification service with in-memory database
- Test data seeding and cleanup mechanisms

### Development & Deployment
- Docker support with multi-stage builds
- Docker Compose configuration for easy deployment
- SQL Server database containerization
- Environment-specific configurations