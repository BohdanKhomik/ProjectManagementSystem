namespace GraduateWork.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public ICollection<ProjectUser>? ProjectUsers { get; set; }
        public List<ProjectColumn> ProjectColumns { get; set; } = new List<ProjectColumn>();
        public List<Sprint>? Sprints { get; set; }
    }
}
