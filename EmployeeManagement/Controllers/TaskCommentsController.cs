using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using EmployeeManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskCommentsController : ControllerBase
{
    private readonly AppDbContext _context;
    public TaskCommentsController(AppDbContext context) => _context = context;

    [HttpGet("task/{projectId}/{taskId}")]
    public async Task<IActionResult> GetByTask(string projectId, string taskId) =>
        Ok(await _context.TaskComments.Where(c => c.ProjectId == projectId && c.TaskId == taskId).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(TaskCommentRequest request)
    {
        if (!await _context.TaskItems.AnyAsync(t => t.ProjectId == request.ProjectId && t.TaskId == request.TaskId)) return BadRequest("Invalid task.");
        if (!string.IsNullOrWhiteSpace(request.EmployeeId) && !await _context.Employees.AnyAsync(e => e.EmployeeId == request.EmployeeId)) return BadRequest("Invalid employee.");

        var comment = new TaskComment
        {
            TaskCommentId = request.TaskCommentId,
            CommentText = request.CommentText,
            CommentDate = request.CommentDate ?? DateTime.Today,
            ProjectId = request.ProjectId,
            TaskId = request.TaskId,
            EmployeeId = string.IsNullOrWhiteSpace(request.EmployeeId) ? null : request.EmployeeId
        };

        _context.TaskComments.Add(comment);
        await _context.SaveChangesAsync();
        return Ok(comment);
    }

    [HttpPut("{taskCommentId}")]
    public async Task<IActionResult> Update(string taskCommentId, UpdateTaskCommentRequest request)
    {
        var comment = await _context.TaskComments.FindAsync(taskCommentId);
        if (comment == null) return NotFound();
        comment.CommentText = request.CommentText;
        await _context.SaveChangesAsync();
        return Ok(comment);
    }

    [HttpDelete("{taskCommentId}")]
    public async Task<IActionResult> Delete(string taskCommentId)
    {
        var comment = await _context.TaskComments.FindAsync(taskCommentId);
        if (comment == null) return NotFound();
        _context.TaskComments.Remove(comment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
