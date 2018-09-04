using Mskj.ArmyKnowledge.All.Common.DataObj;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.LiteFramework.Base;
using Mskj.LiteFramework.Common;
using System.Collections.Generic;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IMsgService : IBaseService<Msg>, IServiceContract
    {
        #region 消息信息
        /// <summary>
        /// 新增消息会话
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        ReturnResult<Msg> AddMsg(Msg msg);
        /// <summary>
        /// 新增消息明细列表
        /// </summary>
        /// <param name="msgDetails"></param>
        /// <returns></returns>
        ReturnResult<MsgDetail> AddMsgDetail(MsgDetail msgDetails);
        /// <summary>
        /// 删除消息会话
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        ReturnResult<bool> DeleteMsg(string msgId);
        /// <summary>
        /// 删除消息明细列表
        /// </summary>
        /// <param name="msgDetailIds"></param>
        /// <returns></returns>
        ReturnResult<bool> DeleteMsgDetails(List<string> msgDetailIds);
        /// <summary>
        /// 获取消息明细列表
        /// </summary>
        /// <param name="filter">查询关键字</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        ReturnResult<IPagedData<MsgDetail>> GetMsgDetail(string msgId, string filter = "",
            int pageIndex = 1, int pageSize = 30);
        /// <summary>
        /// 获取用户的消息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        ReturnResult<IPagedData<MsgModel>> GetUserMsgs(string userid, int pageIndex = 1, int pageSize = 30);
        #endregion
    }
}
