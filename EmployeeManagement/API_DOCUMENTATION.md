# EmployeeManagement API - CRUD Controllers Documentation

## Overview

This document describes the CRUD controllers created for the EmployeeManagement ASP.NET Core Web API. All controllers follow REST conventions and return proper HTTP status codes.

## Base URL
```
api/[controller]
```

## Controllers and Endpoints

### 1. ProjectsController
**Base Route:** `api/projects`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all projects |
| GET | `/{id}` | Get project by ID |
| POST | `/` | Create new project |
| PUT | `/{id}` | Update project |
| DELETE | `/{id}` | Delete project |

**Example:**
```json
POST /api/projects
{
  "projectId": "PROJ001",
  "projectName": "Web Application Development"
}
```

---

### 2. AccountsController
**Base Route:** `api/accounts`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all accounts (password hidden) |
| GET | `/{id}` | Get account by ID (password hidden) |
| POST | `/` | Create new account |
| PUT | `/{id}` | Update account |
| DELETE | `/{id}` | Delete account |

**Example:**
```json
POST /api/accounts
{
  "accountId": "ACC001",
  "username": "john.doe",
  "password": "SecurePassword123"
}
```

**Note:** Password field is never returned in GET responses for security.

---

### 3. EmployeesController
**Base Route:** `api/employees`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all employees with Account and WorkNorm details |
| GET | `/{id}` | Get employee by ID with related entities |
| POST | `/` | Create new employee |
| PUT | `/{id}` | Update employee |
| DELETE | `/{id}` | Delete employee |

**Example:**
```json
POST /api/employees
{
  "employeeId": "EMP001",
  "lastName": "Doe",
  "firstName": "John",
  "email": "john.doe@company.com",
  "phoneNumber": "+1234567890",
  "accountId": "ACC001",
  "workNormId": "WN001"
}
```

**Special Features:**
- Account must be unique per employee
- Includes nested Account and WorkNorm objects in responses
- Validates foreign key references

---

### 4. WorkNormsController
**Base Route:** `api/worknorms`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all work norms |
| GET | `/{id}` | Get work norm by ID |
| POST | `/` | Create new work norm |
| PUT | `/{id}` | Update work norm |
| DELETE | `/{id}` | Delete work norm |

**Example:**
```json
POST /api/worknorms
{
  "workNormId": "WN001",
  "workNormName": "Standard 8-hour workday",
  "workHours": 8
}
```

---

### 5. SkillsController
**Base Route:** `api/skills`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all skills |
| GET | `/{id}` | Get skill by ID |
| POST | `/` | Create new skill |
| PUT | `/{id}` | Update skill |
| DELETE | `/{id}` | Delete skill |

**Example:**
```json
POST /api/skills
{
  "skillId": "SK001",
  "skillName": "C# Programming",
  "skillLevel": 5
}
```

---

### 6. DepartmentsController
**Base Route:** `api/departments`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all departments |
| GET | `/{id}` | Get department by ID |
| POST | `/` | Create new department |
| PUT | `/{id}` | Update department |
| DELETE | `/{id}` | Delete department |

**Example:**
```json
POST /api/departments
{
  "departmentId": "DEPT001",
  "departmentName": "Software Development"
}
```

---

### 7. DescriptionsController
**Base Route:** `api/descriptions`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all task descriptions |
| GET | `/{id}` | Get description by ID |
| POST | `/` | Create new description |
| PUT | `/{id}` | Update description |
| DELETE | `/{id}` | Delete description |

**Example:**
```json
POST /api/descriptions
{
  "descriptionId": "DESC001",
  "taskDescriptionText": "Implement user authentication module"
}
```

---

### 8. TasksController
**Base Route:** `api/tasks`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all tasks with Project and Description details |
| GET | `/{projectId}/{taskId}` | Get task by composite key (ProjectId + TaskId) |
| POST | `/` | Create new task |
| PUT | `/{projectId}/{taskId}` | Update task |
| DELETE | `/{projectId}/{taskId}` | Delete task |

**Example:**
```json
POST /api/tasks
{
  "projectId": "PROJ001",
  "taskId": "TASK001",
  "taskName": "Develop API Endpoints",
  "estimatedHours": 40,
  "descriptionId": "DESC001"
}
```

**Special Features:**
- Composite primary key: ProjectId + TaskId
- GET endpoint uses route parameters for composite key
- Includes nested Project and Description objects in responses
- Validates Project existence before creation

---

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Request successful |
| 201 | Created - Resource successfully created |
| 204 | No Content - Successful update or delete |
| 400 | Bad Request - Invalid input or validation error |
| 404 | Not Found - Resource does not exist |
| 500 | Internal Server Error |

---

## Data Transfer Objects (DTOs)

All controllers use DTOs to separate API responses from database entities.

### Standard DTO Pattern

Each entity has three DTO types:
- **EntityDto**: For GET responses (read-only, without sensitive data)
- **EntityCreateDto**: For POST requests
- **EntityUpdateDto**: For PUT requests

### Account Special Handling

The `AccountDto` used in GET responses does **not** include the password field:
```json
{
  "accountId": "ACC001",
  "username": "john.doe"
}
```

---

## Validation Rules

### Projects
- ProjectId: Required, must be unique
- ProjectName: Required

### Accounts
- AccountId: Required, must be unique
- Username: Required
- Password: Required for creation, optional for update

### Employees
- EmployeeId: Required, must be unique
- LastName: Required
- FirstName: Required
- AccountId: Optional, must be unique if provided
- WorkNormId: Optional

### WorkNorms
- WorkNormId: Required, must be unique
- WorkNormName: Required
- WorkHours: Required, must be > 0

### Skills
- SkillId: Required, must be unique
- SkillName: Required
- SkillLevel: Required, must be > 0

### Departments
- DepartmentId: Required, must be unique
- DepartmentName: Required

### Descriptions
- DescriptionId: Required, must be unique
- TaskDescriptionText: Required

### Tasks
- ProjectId: Required, must reference existing project
- TaskId: Required (composite key with ProjectId)
- TaskName: Required
- EstimatedHours: Required, must be > 0
- DescriptionId: Optional

---

## Error Responses

### Example Bad Request
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "0HN1GVMJ..."
}
```

### Example Not Found
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "traceId": "0HN1GVMJ..."
}
```

---

## Swagger/OpenAPI

All endpoints are automatically documented in Swagger UI.

**Access Swagger UI:**
```
https://localhost:5001/swagger/index.html
```

---

## Implementation Notes

- All controllers use `async/await` for non-blocking operations
- Direct AppDbContext usage (no Repository pattern)
- Proper HTTP status codes and responses
- Input validation on all endpoints
- Handles duplicate key constraints gracefully
- Includes related entities in responses where appropriate
- All timestamps and sensitive data handled appropriately

---

## Future Enhancements

The following features are planned but not yet implemented:
- Authentication and Authorization
- Role-based access control (RBAC)
- Advanced filtering and pagination
- Dependency Injection for services
- Unit tests
- Integration tests
