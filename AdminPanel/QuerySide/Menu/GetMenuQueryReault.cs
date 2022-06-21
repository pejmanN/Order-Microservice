namespace AdminPanel.QuerySide.Menu
{
    public class GetMenuQueryReault
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public bool AuthenticationIsRequired { get; set; }
        public bool Enabled { get; set; }
        public int? ParentId { get; set; }
        public List<GetMenuQueryReault> Menu { get; set; }
    }
}
