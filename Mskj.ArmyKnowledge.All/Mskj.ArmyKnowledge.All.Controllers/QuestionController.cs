using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.ArmyKnowledge.Core;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using QuickShare.LiteFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Mskj.ArmyKnowledge.All.Controllers
{
    [RoutePrefix("Test")]
    public class QuestionController : BaseController
    {
        private readonly IUsersService _GoodsService;

        public QuestionController(IUsersService goodsService)
        {
            _GoodsService = goodsService;
        }
    }
}
