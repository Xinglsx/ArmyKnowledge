using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IFansService : IBaseService<Fans>,IServiceContract
    {
        /// <summary>
        /// 新增粉丝信息
        /// </summary>
        /// <param name="cert">用户认证信息</param>
        ReturnResult<bool> AddFans(Fans fans);
    }
}
