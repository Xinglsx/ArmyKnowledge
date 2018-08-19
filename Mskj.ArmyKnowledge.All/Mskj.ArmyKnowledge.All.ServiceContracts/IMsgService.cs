using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
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
        ReturnResult<List<MsgDetail>> AddMsgDetail(List<MsgDetail> msgDetails);
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
        /// 获取问题列表
        /// </summary>
        /// <param name="filter">查询关键字</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        ReturnResult<IPagedData<MsgDetail>> GetQuestions(string filter = "",
            int pageIndex = 1, int pageSize = 30);
        #endregion

        #region 通知信息
        /// <summary>
        /// 新增通知
        /// </summary>
        ReturnResult<Notice> AddNotice(Notice notice);
        /// <summary>
        /// 更新通知信息
        /// </summary>
        /// <param name="notice">新的通知信息</param>
        ReturnResult<bool> UpdateNotice(Notice notice);
        /// <summary>
        /// 删除通知信息
        /// </summary>
        /// <param name="id">通知ID</param>
        ReturnResult<bool> DeleteNotice(string id);
        /// <summary>
        /// 发布（审核）通知
        /// </summary>
        ReturnResult<bool> AuditNotice(Notice notice);
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="filter">查询关键字</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        ReturnResult<IPagedData<Notice>> GetNotices(string filter = "",
            int pageIndex = 1, int pageSize = 30);
        #endregion
    }
}
