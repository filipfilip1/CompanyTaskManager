# CompanyTaskManager (ASP.NET Core MVC)

A web application written in ASP.NET Core (MVC architecture) for managing tasks and projects in small teams.

## Table of contents

1. [Functions and roles](#functions-and-roles)
2. [Technologies and architecture](#technologies-and-architecture)
3. [Project structure](#project-structure)
4. [Launching an application using Docker](#launching-an-application-using-docker)
5. [Development plans](#development-plans)


---
![Image](https://github.com/user-attachments/assets/a9cc41b3-4060-46ec-a2b9-8f2e55469d19)

## Functions and roles
- **Registration and login** of users, with choice of role `Employee` or `Manager`.
  - In addition, `Administrator` is a seeded user and it is not possible to assign such a role through registration.

### Administrator
- Approves role requests (Employee / Manager) after registration of new users.
  
  ![Image](https://github.com/user-attachments/assets/71690e69-c2e5-4d33-a543-6d4d73849ed6)
- Can block (deactivate) user accounts.


### Manager
- Creates **teams** and manages them (adding/deleting employees).
  
 ![Image](https://github.com/user-attachments/assets/61bdc206-1304-498c-844d-2b23716a4684)
- Can create **tasks** and **projects** (assigns them to team members, appoints a project leader).
  
 ![Image](https://github.com/user-attachments/assets/3fea92e4-c61b-43fd-b71b-d2d7d14ceb1b)
- Approves or rejects tasks and projects (possible only when all tasks in the project are completed).

### Employee
- Reviews **tasks and projects** to which he/she is assigned.
  
  ![Image](https://github.com/user-attachments/assets/f54925b1-1e42-4c76-8c5f-34f3e3f47c40)
- **Updates** statuses and sends tasks for approval.  


### Additional features
- **Notification System** (for Manager and Employee) - informs, for example, about assignment to a task, sending a task for approval etc.
  
  ![Image](https://github.com/user-attachments/assets/44cee2fa-1b9b-4590-a7d0-7d2032d3636b)
- **Calendar (FullCalendar)** - interactive preview of active tasks and projects.
  
 ![Image](https://github.com/user-attachments/assets/cbf011be-20f2-4fcf-8d10-2a8ffdfd574d)

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
   git clone https://github.com/filipfilip1/CompanyTaskManager.git
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
