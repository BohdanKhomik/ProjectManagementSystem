using GraduateWork.Enums;

namespace GraduateWork.Models
{
    public class ProjectUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Project? Project { get; set; }
        public int ProjectId { get; set; }

        public Roles Role { get; set; }
    }
}
