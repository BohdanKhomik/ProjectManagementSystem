using GraduateWork.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GraduateWork.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectColumn> ProjectColumns { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId);

            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(au => au.ProjectUsers)
                .HasForeignKey(pu => pu.UserId);

            builder.Entity<Project>()
                .HasMany(p => p.Sprints)
                .WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasMany(p => p.ProjectColumns)
                .WithOne(pc => pc.Project)
                .HasForeignKey(pc => pc.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProjectColumn>()
                .HasMany(pc => pc.Issues)
                .WithOne(i => i.ProjectColumn)
                .HasForeignKey(i => i.ColumnId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sprint>()
                .HasMany(s => s.Issues)
                .WithOne(i => i.Sprint)
                .HasForeignKey(i => i.SprintId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Issue>()
                .HasOne(i => i.Assignee)
                .WithMany(u => u.AssignedIssues)
                .HasForeignKey(i => i.AssigneeUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Issue>()
                .HasOne(i => i.Reporter)
                .WithMany(u => u.ReportedIssues)
                .HasForeignKey(i => i.ReporterUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}