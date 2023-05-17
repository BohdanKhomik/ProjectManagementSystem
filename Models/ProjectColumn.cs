namespace GraduateWork.Models
{
    public class ProjectColumn
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public Project? Project { get; set; }
        public int ProjectId { get; set; }

        public List<Issue>? Issues { get; set; }
    }
}
