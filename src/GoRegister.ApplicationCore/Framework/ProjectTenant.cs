namespace GoRegister.ApplicationCore.Framework
{
    public class ProjectTenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public string Hostname { get; set; }
        public string Prefix { get; set; }

        public bool IsProjectAdmin => IsAdmin && Id != 0; // 0 = admin project

        public ProjectTenant Clone(int id)
        {
            return new ProjectTenant()
            {
                Id = id,
                Name = Name,
                IsAdmin = IsAdmin,
                Hostname = Hostname,
                Prefix = Prefix
            };
        }
    }
}
