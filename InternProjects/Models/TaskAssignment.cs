namespace InternProjects.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public int TaskId { get; set; }
        public TaskItem? Task { get; set;  }
        
        public int InternId { get; set; }
        public Intern? Intern { get; set; }

        public List<Submission> Submissions { get; set; } = new();
        public List<TimeLog> TimeLogs { get; set; } = new();

    }
}
