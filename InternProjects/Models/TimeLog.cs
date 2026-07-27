using Microsoft.Identity.Client;

namespace InternProjects.Models
{
    public class TimeLog
    {
        public int Id { get; set; }
        public string SourceType { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string StatusApproval { get; set; }
        public DateTime CreationDate {  get; set; }

        public int InternId {get; set; }
        public Intern? Intern { get; set; }

        public int? AssignmentId { get; set; }
        public TaskAssignment? Assignment { get; set; }


        public int? CreatedById { get; set; }
        public User? CreatedBy {  get; set; }

        public int? ApprovedById { get; set; }
        public User? ApprovedBy { get; set; }

    }
}
