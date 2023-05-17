namespace GraduateWork.Models
{
    public class Sprint
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Project? Project { get; set; }

        public int ProjectId { get; set; }
        public List<Issue>? Issues { get; set;}
    }
}
