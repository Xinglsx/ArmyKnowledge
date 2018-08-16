using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using System;
using System.Web.Http;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;

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
        
        [Route("AddUser")]
        [HttpPost]
        public object AddUser(Users user)
        {
            bool saveResult = false;
            try
            {
                saveResult = _GoodsService.Add(user);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, exp.Message);
            }
            return new ReturnResult<bool>(1,saveResult);
        }
        
    }
}
