using Microsoft.AspNetCore.Identity;

namespace GraduateWork.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ProjectUser>? ProjectUsers { get; set; }
        public virtual ICollection<Issue>? AssignedIssues { get; set; }
        public virtual ICollection<Issue>? ReportedIssues { get; set; }
    }
}
