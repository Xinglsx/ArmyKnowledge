using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using System.Collections.Generic;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface INoticeService : IBaseService<Notice>, IServiceContract
    {
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
