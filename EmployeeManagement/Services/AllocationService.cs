using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using EmployeeManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services;

public class AllocationService : IAllocationService
{
    private readonly AppDbContext _context;

    public AllocationService(AppDbContext context)
    {
        _context = context;
    }

    public int CountWorkingDays(DateTime start, DateTime end)
    {
        if (end < start) return 0;
        var count = 0;
        for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                count++;
        }
        return count;
    }

    public decimal CalculateTotalAllocationHours(DateTime start, DateTime end, decimal hoursPerDay)
    {
        return CountWorkingDays(start, end) * hoursPerDay;
    }

    public bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
    {
        return start1.Date <= end2.Date && start2.Date <= end1.Date;
    }

    public async Task<decimal> GetEmployeeAllocatedHoursForDateAsync(string employeeId, DateTime date)
    {
        return await _context.Allocations
            .Where(a => a.EmployeeId == employeeId &&
                        a.AllocationStartDate.Date <= date.Date &&
                        (a.AllocationEndDate ?? a.AllocationStartDate).Date >= date.Date)
            .SumAsync(a => a.AllocatedHours);
    }

    public async Task<List<AllocationResponse>> GetAllAsync() => await BuildAllocationQuery().ToListAsync();

    public async Task<List<AllocationResponse>> GetByEmployeeAsync(string employeeId) =>
        await BuildAllocationQuery().Where(a => a.EmployeeId == employeeId).ToListAsync();

    public async Task<List<AllocationResponse>> GetByProjectAsync(string projectId) =>
        await BuildAllocationQuery().Where(a => a.ProjectId == projectId).ToListAsync();

    public async Task<List<AllocationResponse>> GetByTaskAsync(string projectId, string taskId) =>
        await BuildAllocationQuery().Where(a => a.ProjectId == projectId && a.TaskId == taskId).ToListAsync();

    public async Task<(bool Success, string? Error, AllocationResponse? Allocation)> CreateAllocationAsync(CreateAllocationRequest request)
    {
        var employee = await _context.Employees.Include(e => e.WorkNorm)
            .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId);
        if (employee?.WorkNorm == null) return (false, "Invalid employee or work norm.", null);

        var task = await _context.TaskItems.Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectId == request.ProjectId && t.TaskId == request.TaskId);
        if (task == null) return (false, "Invalid task.", null);

        var endDate = request.AllocationEndDate ?? request.AllocationStartDate;
        if (request.AllocationStartDate.Date > endDate.Date) return (false, "Invalid interval.", null);
        if (request.AllocatedHours <= 0) return (false, "Invalid hours.", null);

        var duplicate = await _context.Allocations.FindAsync(request.EmployeeId, request.ProjectId, request.TaskId);
        if (duplicate != null) return (false, "Duplicate allocation.", null);

        var newTotalHours = CalculateTotalAllocationHours(request.AllocationStartDate, endDate, request.AllocatedHours);
        var existingAllocations = await _context.Allocations
            .Where(a => a.ProjectId == request.ProjectId && a.TaskId == request.TaskId)
            .ToListAsync();

        var currentTotal = existingAllocations.Sum(a =>
            CalculateTotalAllocationHours(a.AllocationStartDate, a.AllocationEndDate ?? a.AllocationStartDate, a.AllocatedHours));

        var estimatedHours = task.EstimatedHours ?? 0;
        if (estimatedHours > 0 && currentTotal + newTotalHours > estimatedHours)
            return (false, "Task hours exceeded.", null);

        for (var date = request.AllocationStartDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;
            var dailyHours = await GetEmployeeAllocatedHoursForDateAsync(request.EmployeeId, date);
            if (dailyHours + request.AllocatedHours > employee.WorkNorm.WorkHours)
                return (false, "Work norm exceeded.", null);
        }

        var allocation = new Allocation
        {
            EmployeeId = request.EmployeeId,
            ProjectId = request.ProjectId,
            TaskId = request.TaskId,
            AllocationStartDate = request.AllocationStartDate.Date,
            AllocationEndDate = endDate.Date,
            AllocatedHours = request.AllocatedHours
        };

        _context.Allocations.Add(allocation);
        await _context.SaveChangesAsync();

        var response = (await GetByTaskAsync(request.ProjectId, request.TaskId))
            .First(a => a.EmployeeId == request.EmployeeId);
        response.TotalAllocationHours = newTotalHours;
        return (true, null, response);
    }

    private IQueryable<AllocationResponse> BuildAllocationQuery()
    {
        return _context.Allocations
            .Include(a => a.Employee)
            .Include(a => a.Project)
            .Include(a => a.TaskItem)
            .Select(a => new AllocationResponse
            {
                EmployeeId = a.EmployeeId,
                ProjectId = a.ProjectId,
                TaskId = a.TaskId,
                EmployeeName = a.Employee == null ? null : a.Employee.LastName + " " + a.Employee.FirstName,
                ProjectName = a.Project == null ? null : a.Project.ProjectName,
                TaskName = a.TaskItem == null ? null : a.TaskItem.TaskName,
                AllocationStartDate = a.AllocationStartDate,
                AllocationEndDate = a.AllocationEndDate,
                AllocatedHours = a.AllocatedHours,
                TotalAllocationHours = 0
            });
    }
}
