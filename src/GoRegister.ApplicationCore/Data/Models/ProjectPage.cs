namespace GoRegister.ApplicationCore.Data.Models
{
    public class ProjectPage : IMustHaveProject
    {
        public enum PageType
        {
            Registration,
            Custom
        }

        public int Id { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public PageType Type { get; set; }

        public int MenuPosition { get; set; }

        public CustomPage CustomPage { get; set; }

    }
}
