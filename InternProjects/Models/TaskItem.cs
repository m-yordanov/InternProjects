using Microsoft.Identity.Client;

namespace InternProjects.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? SubmissionFormat { get; set; }
        public float AssignedHours { get; set; }
        public string? Difficulty { get; set; }
        public string? Priority { get; set; }
        public string Status { get; set; }
        public string? SuitableFor { get; set; }
        public bool IsTeamTask { get; set; }
        public int MaxInterns { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int CreatorId { get; set; }
        public User? Creator { get; set; }

        public List<TaskAssignment> Assignments { get; set; } = new();
    }
}
