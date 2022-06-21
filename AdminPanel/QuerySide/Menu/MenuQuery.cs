using AdminPanel.CommandSide.Infra;
using AdminPanel.QuerySide.Menu;

namespace AdminPanel.QuerySide
{
    public class MenuQuery : IMenuQuery
    {
        private readonly PanelDbContextt _dbContext;

        public MenuQuery(PanelDbContextt dbContext)
        {
            _dbContext = dbContext;
        }

        public List<GetMenuQueryReault> GetMenus()
        {
            var menus = _dbContext.Menus.ToList();
            return MapToQueryMenu(menus);
        }

        private List<GetMenuQueryReault> MapToQueryMenu(IReadOnlyCollection<CommandSide.Domain.Menu.Menu> children)
        {
            var result = new List<GetMenuQueryReault>();
            foreach (var menu in children)
            {
                result.Add(new GetMenuQueryReault
                {
                    AuthenticationIsRequired = menu.AuthenticationIsRequired,
                    Enabled = menu.Enabled,
                    Id = menu.Id,
                    ParentId = menu.ParentId,
                    Path = menu.Path,
                    Title = menu.Title,
                    Menu = MapToQueryMenu(menu.Children)
                });
            }

            return result;
        }
    }
}
