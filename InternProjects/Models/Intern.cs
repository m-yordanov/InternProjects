using Microsoft.Identity.Client;

namespace InternProjects.Models
{
    public class Intern
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string University { get; set; }
        public string Specialty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float TotalHours { get; set; } = 240;
        public float TaskHours { get; set; }
        public float AddedHours { get; set; }
        public float ReportedHours { get; set; }
        public float RemainingHours { get; set; }
        public bool StatusAccount { get; set; }
        public string? Notes { get; set; }

        public int? MentorId { get; set; }
        public User? Mentor { get; set; }

        public List<TaskAssignment>? Assignments { get; set; }
    }
}
