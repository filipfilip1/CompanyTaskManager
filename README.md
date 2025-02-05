# CompanyTaskManager (ASP.NET Core MVC)

A web application written in ASP.NET Core (MVC architecture) for managing tasks and projects in small teams.

## Table of contents

1. [Functions and roles](#functions-and-roles)
2. [Technologies and architecture](#technologies-and-architecture).
3. [Project structure](#project-structure).
4. [Launching an application using Docker](#launching-an-application-using-docker).
5. [Development plans](#development-plans).


---

## Functions and roles

- **Registration and login** of users, with choice of role `Employee` or `Manager`.  
  - In addition, `Administrator` is a seeded user and it is not possible to assign such a role through registration.

### Administrator
- Approves role requests (Employee / Manager) after registration of new users.  
- Can block (deactivate) user accounts.  

### Manager
- Creates **teams** and manages them (adding/deleting employees).  
- Can create **tasks** and **projects** (assigns them to team members, appoints a project leader).  
- Approves or rejects tasks and projects (possible only when all tasks in the project are completed).

### Employee
- Reviews **tasks and projects** to which he/she is assigned.  
- **Updates** statuses and sends tasks for approval.  


### Additional features
- **Notification System** (for Manager and Employee) - informs, for example, about assignment to a task, sending a task for approval etc.  
- **Calendar (FullCalendar)** - interactive preview of active tasks and projects.

---

## Technologies and architecture

- **C# / .NET** - in the services and business logic layer.
- **ASP.NET Core 8 (MVC)** - presentation layer (controllers, views).  
- **Entity Framework Core** - object-relational mapping (ORM).  
- **MS SQL Server** - database.  
- **Identity** - registration, login, user role management.  
- **FullCalendar (JS)** - interactive calendar in the frontend.  
- **AutoMapper** - mapping domain entities to ViewModels.  
- **Docker** - multi-stage image building (`Dockerfile`) and composition using `docker-compose.yml` (ability to run the entire application in a container).

The application is based on **MVC (Model-View-Controller)** pattern with separation of logic in separate layers/projects.

---

## Project structure

The solution (Solution) consists of **4** related projects:

1. **Web**.  
   - MVC controllers.  
   - Razor views.  

2. **Application**.  
   - Services and business logic.  
   - ViewModels and AutoMapper configurations.  

3. **Data**.  
   - **Domain Entities**.  
   - **Entity Framework** (`ApplicationDbContext`, Fluent API configurations).  
   - Database migrations.

4. **Common**.  
   - Constant definitions of roles (`Roles`) and statuses (`WorkStatuses`).
     
---

## Launching an application using Docker
1. **Clone the repository**

   ```sh
   git clone https://github.com/twoj-uzytkownik/twoj-projekt.git
   ```

2. **Navigate to the project directory**

3. **Start the containers**

   ```sh
   docker-compose up -d
   ```

4. **Access the application**

   - The application should be available at:  
     [http://localhost:8080](http://localhost:8080)

### Seeded User Accounts

When you launch the application for the first time, a set of default user accounts is automatically created in the database:

| Role          | Email                     | Password    |
|--------------|-------------------------|-------------|
| Administrator | `admin@localhost.com`   | `P@ssword1` |
| Manager      | `manager@localhost.com`  | `P@ssword1` |
| Employees    | `employee1@localhost.com`  | `P@ssword1` |
|              | `employee2@localhost.com`  | `P@ssword1` |
|              | `employee3@localhost.com`  | `P@ssword1` |

###  Stopping the Application

To stop the application and remove the containers, run:

```sh
docker-compose down
```


## Development plans
 - Unit and integration testing. 
 - SignalR - expansion of the notification system to include real-time communication.
 - File Upload - ability to attach files to tasks/projects.
