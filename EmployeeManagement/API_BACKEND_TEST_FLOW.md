# Backend test flow

Run the API and open Swagger.

Recommended order:

1. Create WorkNorm
2. Create Account
3. Create Employee
4. Create Project
5. Create TaskDescription
6. Create TaskItem
7. Create Allocation
8. Add Timesheet entry
9. Add TaskComment
10. Check employee workload dashboard
11. Check task progress dashboard
12. Check project summary dashboard

Important composite keys:

- TaskItem: ProjectId + TaskId
- Allocation: EmployeeId + ProjectId + TaskId
- Timesheet: ProjectId + TaskId + EmployeeId + WorkDate
- EmployeeSkill: EmployeeId + SkillId
- EmployeeDepartment: EmployeeId + DepartmentId
- ProjectManager: EmployeeId + ProjectId

The backend uses the existing SQL Server database. Do not run migrations for this version.
