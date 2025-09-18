TaskManagementApi

TaskManagementApi is a role-based task management system built with ASP.NET Core Web API. It demonstrates authentication with JWT tokens, secure password storage, role-based authorization, and middleware logging.

📂 Domain Model Overview

User

Id, Email, PasswordHash, RoleId

Navigation: Profile, Projects, Tasks, Assignments

UserProfile (1:1 with User)

UserId, FullName, Phone, Address

Project (1:N with Tasks, M:N with Members)

Id, Name, Description, OwnerId

Navigation: Tasks, Members

TaskItem (1:N with Project, assigned to User)

Id, Title, Description, ProjectId, AssignedToUserId, Status, Priority

Navigation: Assignments, Tags (optional)

TaskAssignment (History of task assignments)

Id, TaskItemId, AssignedToUserId, AssignedByUserId, AssignedAt, Comment

(History table, not directly exposed for CRUD)

Tag (optional M:N with Tasks)

Id, Name, ColorHex

TaskTag (join table between TaskItem and Tag)

TaskItemId, TagId

🔐 Authentication & Authorization

Authentication: JWT Bearer tokens

Password Security: Hashed passwords (no plain text)

Authorization Roles:

Admin → Full user management rights

Manager → Create/edit projects, manage project members

Developer → Work with tasks assigned to them

🛠️ Middleware Logging

A custom middleware logs:

X-Correlation-ID

HTTP method

Path

Status code

🗂️ Seeded Users (for Login Testing)

Since registration is not enabled (admin-only responsibility), initial users are seeded into the database. Use these credentials to test authentication:

Super Admin

{
  "email": "superadmin@example.com",
  "password": "SuperAdmin123!"
}


Manager

{
  "email": "sampleManager@example.com",
  "password": "Manager@123"
}


Developer 1

{
  "email": "sampleDevelaper@example.com",
  "password": "Dev@123"
}


Developer 2

{
  "email": "sampleDevelaper2@example.com",
  "password": "2Dev@123"
}

▶️ Running the Project (Visual Studio)

Open the solution in Visual Studio.

Right-click TaskManagementApi → Set as Startup Project.

Press F5 to run.

Swagger UI will be available at:

https://localhost:<port>/swagger

🔑 How to Test Authentication

Use Swagger UI or Postman.

Login with one of the seeded user credentials via:

POST /api/auth/login


Example request body:

{
  "email": "superadmin@example.com",
  "password": "SuperAdmin123!"
}


Copy the returned JWT token.

In Swagger, click Authorize and enter:

Bearer <your-token>


Access protected endpoints depending on the user’s role.

⚡ Notes

TaskAssignment table is for history only (not CRUD exposed).

Roles are hardcoded/seeded and linked to users.

Example roles to test:

Admin → Manage users

Manager → Manage projects and members

Developer → Manage own tasks
