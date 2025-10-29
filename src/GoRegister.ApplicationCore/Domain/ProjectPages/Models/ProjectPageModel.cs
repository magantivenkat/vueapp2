using GoRegister.ApplicationCore.Data.Models;

namespace GoRegister.ApplicationCore.Domain.ProjectPages.Models
{
    public class ProjectPageModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public ProjectPage.PageType Type { get; set; }

        public string Title { get; set; }

        public bool IsVisible { get; set; }

        public int MenuPosition { get; set; }

        public int PageId { get; set; }
    }
}
