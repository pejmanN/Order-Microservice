namespace AdminPanel.CommandSide.Domain.Menu
{
    public class Menu
    {
        private List<Menu> _children = new List<Menu>();

        public int Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public bool AuthenticationIsRequired { get; set; }
        public bool Enabled { get; set; }
        public int? ParentId { get; set; }

        public IReadOnlyCollection<Menu> Children => _children.ToList();

        protected Menu() { }

        public Menu(int id, string title, string path, bool authenticationIsRequired, bool enabled, int? parentId)
        {
            Id = id;
            Title = title;
            Path = path;
            AuthenticationIsRequired = authenticationIsRequired;
            Enabled = enabled;
            ParentId = parentId;

            //should raise the related Event
        }

        public void AddChild(int id, string name, string path, bool authenticationIsRequired, bool enabled, int? parentId)
        {
            throw new NotImplementedException();
        }
    }
}
