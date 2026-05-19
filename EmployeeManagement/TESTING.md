# Backend verification

Run these commands from the repository root:

```bash
dotnet restore EmployeeManagement/EmployeeManagement.csproj
dotnet build EmployeeManagement/EmployeeManagement.csproj
dotnet test EmployeeManagement.Tests/EmployeeManagement.Tests.csproj
```

Swagger manual test flow:

1. Create WorkNorm
2. Create Account
3. Create Employee
4. Create Project
5. Create TaskDescription
6. Create TaskItem
7. Create Allocation
8. Add Timesheet
9. Add TaskComment
10. Check Dashboard endpoints

Dashboard endpoints:

- GET `/api/dashboard/employee/{employeeId}/workload`
- GET `/api/dashboard/task-progress/{projectId}/{taskId}`
- GET `/api/dashboard/project/{projectId}/summary`

Important: the API uses the existing SQL Server database. Do not run migrations for this version.
