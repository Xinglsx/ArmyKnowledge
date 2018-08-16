using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using System;
using System.Web.Http;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Mskj.ArmyKnowledge.All.Controllers
{
    [RoutePrefix("Users")]
    public class UserController : BaseController
    {
        private readonly IUsersService _GoodsService;

        public UserController(IUsersService goodsService)
            :base()
        {
            _GoodsService = goodsService;
        }
        
        [Route("GetRecommandGoodsList")]
        [HttpGet]
        public object GetRecommandGoodsList(int curPage, int pageSize, int type, string filter)
        {
            return "";
        }
        
    }
}
