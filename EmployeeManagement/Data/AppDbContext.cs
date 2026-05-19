using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkNorm> WorkNorms { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TaskDescription> TaskDescriptions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public DbSet<Allocation> Allocations { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public DbSet<ProjectManager> ProjectManagers { get; set; }
        public DbSet<TaskPeriod> TaskPeriods { get; set; }
        public DbSet<ProjectPeriod> ProjectPeriods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Project table
            modelBuilder.Entity<Project>()
                .ToTable("Project")
                .HasKey(p => p.ProjectId);

            // Configure WorkNorm table
            modelBuilder.Entity<WorkNorm>()
                .ToTable("WorkNorm")
                .HasKey(w => w.WorkNormId);

            // Configure Skill table
            modelBuilder.Entity<Skill>()
                .ToTable("Skill")
                .HasKey(s => s.SkillId);

            // Configure Department table
            modelBuilder.Entity<Department>()
                .ToTable("Department")
                .HasKey(d => d.DepartmentId);

            // Configure Account table
            modelBuilder.Entity<Account>()
                .ToTable("Account")
                .HasKey(a => a.AccountId);

            // Configure TaskDescription table
            modelBuilder.Entity<TaskDescription>()
                .ToTable("TaskDescription")
                .HasKey(td => td.TaskDescriptionId);

            // Configure Employee table
            modelBuilder.Entity<Employee>()
                .ToTable("Employee")
                .HasKey(e => e.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.WorkNorm)
                .WithMany()
                .HasForeignKey(e => e.WorkNormId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure TaskItem table with composite key
            modelBuilder.Entity<TaskItem>()
                .ToTable("TaskItem")
                .HasKey(t => new { t.ProjectId, t.TaskId });

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Project)
                .WithMany(p => p.TaskItems)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TaskComment table
            modelBuilder.Entity<TaskComment>()
                .ToTable("TaskComment")
                .HasKey(tc => tc.TaskCommentId);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.TaskItem)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => new { tc.ProjectId, tc.TaskId })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Employee)
                .WithMany(e => e.TaskComments)
                .HasForeignKey(tc => tc.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Period table
            modelBuilder.Entity<Period>()
                .ToTable("Period")
                .HasKey(p => p.PeriodId);

            // Configure Timesheet table with composite key
            modelBuilder.Entity<Timesheet>()
                .ToTable("Timesheet")
                .HasKey(t => new { t.ProjectId, t.TaskId, t.EmployeeId, t.EntryDate });

            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Timesheets)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.TaskItem)
                .WithMany(ti => ti.Timesheets)
                .HasForeignKey(t => new { t.ProjectId, t.TaskId })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.Employee)
                .WithMany(e => e.Timesheets)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure EmployeeSkill table with composite key
            modelBuilder.Entity<EmployeeSkill>()
                .ToTable("EmployeeSkill")
                .HasKey(es => new { es.EmployeeId, es.SkillId });

            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeSkills)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Skill)
                .WithMany(s => s.EmployeeSkills)
                .HasForeignKey(es => es.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Allocation table with composite key
            modelBuilder.Entity<Allocation>()
                .ToTable("Allocation")
                .HasKey(a => new { a.EmployeeId, a.ProjectId, a.TaskId });

            modelBuilder.Entity<Allocation>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Allocations)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Allocation>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Allocations)
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Allocation>()
                .HasOne(a => a.TaskItem)
                .WithMany(t => t.Allocations)
                .HasForeignKey(a => new { a.ProjectId, a.TaskId })
                .OnDelete(DeleteBehavior.Cascade);

            // Configure EmployeeDepartment table with composite key
            modelBuilder.Entity<EmployeeDepartment>()
                .ToTable("EmployeeDepartment")
                .HasKey(ed => new { ed.EmployeeId, ed.DepartmentId });

            modelBuilder.Entity<EmployeeDepartment>()
                .HasOne(ed => ed.Employee)
                .WithMany(e => e.EmployeeDepartments)
                .HasForeignKey(ed => ed.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeDepartment>()
                .HasOne(ed => ed.Department)
                .WithMany(d => d.EmployeeDepartments)
                .HasForeignKey(ed => ed.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ProjectManager table with composite key
            modelBuilder.Entity<ProjectManager>()
                .ToTable("ProjectManager")
                .HasKey(pm => new { pm.EmployeeId, pm.ProjectId });

            modelBuilder.Entity<ProjectManager>()
                .HasOne(pm => pm.Employee)
                .WithMany(e => e.ProjectManagers)
                .HasForeignKey(pm => pm.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectManager>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectManagers)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TaskPeriod table with composite key
            modelBuilder.Entity<TaskPeriod>()
                .ToTable("TaskPeriod")
                .HasKey(tp => new { tp.ProjectId, tp.TaskId, tp.PeriodId });

            modelBuilder.Entity<TaskPeriod>()
                .HasOne(tp => tp.Project)
                .WithMany(p => p.TaskPeriods)
                .HasForeignKey(tp => tp.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskPeriod>()
                .HasOne(tp => tp.TaskItem)
                .WithMany(t => t.TaskPeriods)
                .HasForeignKey(tp => new { tp.ProjectId, tp.TaskId })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskPeriod>()
                .HasOne(tp => tp.Period)
                .WithMany(p => p.TaskPeriods)
                .HasForeignKey(tp => tp.PeriodId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ProjectPeriod table with composite key
            modelBuilder.Entity<ProjectPeriod>()
                .ToTable("ProjectPeriod")
                .HasKey(pp => new { pp.ProjectId, pp.PeriodId });

            modelBuilder.Entity<ProjectPeriod>()
                .HasOne(pp => pp.Project)
                .WithMany(p => p.ProjectPeriods)
                .HasForeignKey(pp => pp.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectPeriod>()
                .HasOne(pp => pp.Period)
                .WithMany(p => p.ProjectPeriods)
                .HasForeignKey(pp => pp.PeriodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}