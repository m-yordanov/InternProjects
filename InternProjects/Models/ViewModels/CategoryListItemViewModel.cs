namespace InternProjects.Models.ViewModels
{
    public class CategoryListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int TaskCount { get; set; }
    }
}