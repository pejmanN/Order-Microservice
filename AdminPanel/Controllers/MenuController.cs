using AdminPanel.QuerySide;
using AdminPanel.QuerySide.Menu;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuQuery _queryRepo;
        public MenuController(ILogger<MenuController> logger, IMenuQuery queryRepo)
        {
            _logger = logger;
            _queryRepo = queryRepo;
        }

        [HttpGet]
        public List<GetMenuQueryReault> Get()
        {
            //var t = new GetMenuQueryReault();
            //t.Title = "p";
            //t.Title ??= "peji";

            return new List<GetMenuQueryReault>
            {
                new GetMenuQueryReault
                {
                    Id = 1,
                    Title ="Cloth",
                    ParentId = null,
                    Path = "/a",
                    Enabled = true,
                    AuthenticationIsRequired=false,
                    Menu= new List<GetMenuQueryReault>
                    {
                         new GetMenuQueryReault
                         {
                             Id = 2,
                             Title ="Male",
                             ParentId = 1,
                             Path = "/a",
                             Enabled = true,
                             AuthenticationIsRequired=false,
                             Menu= new List<GetMenuQueryReault>
                             {
                                 new GetMenuQueryReault
                                 {
                                     Id = 5,
                                     Title ="Shirt",
                                     ParentId = 2,
                                     Path = "/a",
                                     Enabled = true,
                                     AuthenticationIsRequired=false,
                                     Menu= new List<GetMenuQueryReault>
                                     {
                                         new GetMenuQueryReault
                                         {
                                             Id = 13,
                                             Title ="blue shirt",
                                             ParentId = 5,
                                             Path = "/a",
                                             Enabled = true,
                                             AuthenticationIsRequired=false,
                                         }
                                     }
                                 },
                              }
                         },

                         new GetMenuQueryReault
                         {
                             Id = 3,
                             Title ="female",
                             ParentId = 1,
                             Path = "/a",
                             Enabled = true,
                             AuthenticationIsRequired=false,
                         },
                    }
                },
                 new GetMenuQueryReault
                  {
                      Id = 10,
                      Title ="Eletrical Device",
                      ParentId = null,
                      Path = "/a",
                      Enabled = true,
                      AuthenticationIsRequired=false,
                      Menu= new List<GetMenuQueryReault>
                             {
                                 new GetMenuQueryReault
                                 {
                                     Id = 16,
                                     Title ="Shirt",
                                     ParentId = 10,
                                     Path = "/a",
                                     Enabled = true,
                                     AuthenticationIsRequired=false,
                                     Menu= new List<GetMenuQueryReault>
                                     {
                                         new GetMenuQueryReault
                                         {
                                             Id = 17,
                                             Title ="TV",
                                             ParentId = 16,
                                             Path = "/a",
                                             Enabled = true,
                                             AuthenticationIsRequired=false,
                                         }
                                     }
                                 },
                              }
                   }
            };
            //var result = _queryRepo.GetMenus();
            //return result;
        }
    }
}