using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IMsgService : IBaseService<Msg>, IServiceContract
    {
        /// <summary>
        /// 新增消息信息
        /// </summary>
        ReturnResult<bool> AddMsg(Msg msg);
    }
}
