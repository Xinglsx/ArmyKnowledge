using Mskj.ArmyKnowledge.All.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using QuickShare.LiteFramework.Base;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Common;
using QuickShare.LiteFramework.Common.Extenstions;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Mskj.ArmyKnowledge.Core;
using Mskj.ArmyKnowledge.All.Domains;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class UsersService : BaseService<Users, Users>,IUsersService
    {
        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public UsersService(IRepository<Users> goodsRepository):base(goodsRepository)
        {
        }

        public object GetRecentGoodsList()
        {
            throw new NotImplementedException();
        }
    }
}
