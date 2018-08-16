using Mskj.ArmyKnowledge.All.Domains;
using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IUsersService : IBaseService<Users>,IServiceContract
    {
        /// <summary>
        /// 获取最近三天内的十条管理员推荐的商品信息
        /// </summary>
        /// <returns></returns>
        object GetRecentGoodsList();
    }
}
