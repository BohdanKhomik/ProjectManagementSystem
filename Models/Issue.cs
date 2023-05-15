namespace GraduateWork.Models
{
    public class Issue
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public ProjectColumn ProjectColumn { get; set; }

        public int ColumnId { get; set; }

        public Sprint Sprint { get; set; }

        public int SprintId { get; set; }

        public TimeSpan EstimatedTime { get; set; }
        public TimeSpan EllapsedTime { get; set; }

        public DateTime CreatedDate { get; set; }

        public string AssigneeUserId { get; set; }
        public string ReporterUserId { get; set; }
        public ApplicationUser Assignee { get; set; }
        public ApplicationUser Reporter { get; set; }
    }

}
