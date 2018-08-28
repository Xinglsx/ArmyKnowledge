using Mskj.ArmyKnowledge.All.ServiceContracts;
using QuickShare.LiteFramework.Base;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using QuickShare.LiteFramework.Common;
using System.Linq.Expressions;
using System.Data.SqlClient;
using QuickShare.LiteFramework.Common.Extenstions;
using Jiguang.JPush.Model;
using Jiguang.JPush;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class NoticeService : BaseService<Notice, Notice>, INoticeService
    {
        #region 构造函数
        private readonly IRepository<Notice> _NoticeRepository;
        private static JPushClient client = new JPushClient("2e6a365d5d05e1097e38339c", "af70172e784fd7209373746d");

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public NoticeService(IRepository<Notice> noticeRepository) :
            base(noticeRepository)
        {
            this._NoticeRepository = noticeRepository;
        }
        #endregion

        #region 通知信息
        /// <summary>
        /// 新增通知
        /// </summary>
        public ReturnResult<Notice> AddNotice(Notice notice)
        {
            notice.id = Guid.NewGuid().ToString();
            notice.noticestate = 0;//新增状态
            notice.publishtime = DateTime.Now;
            bool saveResult = false;
            try
            {
                saveResult = _NoticeRepository.Add(notice);
            }
            catch (Exception exp)
            {
                return new ReturnResult<Notice>(-1, exp.Message);
            }
            if (saveResult)
            {
                //消息保存成功，发起推送
                PushPayload pushPayload = new PushPayload()
                {
                    Platform = new List<string> { "android" },
                    Audience = "all",
                    Message = new Message
                    {
                        Title = notice.title,
                        Content = notice.content,
                    },
                };
                var response = client.SendPush(pushPayload);
                return new ReturnResult<Notice>(1, notice);
            }
            else
            {
                return new ReturnResult<Notice>(-2, "通知发布保存失败！");
            }
        }
        /// <summary>
        /// 更新通知信息
        /// </summary>
        /// <param name="notice">新的通知信息</param>
        public ReturnResult<bool> UpdateNotice(Notice notice)
        {
            bool updateResult = false;
            try
            {
                updateResult = _NoticeRepository.Update(notice);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, false, exp.Message);
            }
            if (updateResult)
            {
                return new ReturnResult<bool>(1, updateResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, updateResult, "通知信息更新失败！");
            }
        }
        /// <summary>
        /// 删除通知信息
        /// </summary>
        /// <param name="id">通知ID</param>
        public ReturnResult<bool> DeleteNotice(string id)
        {
            bool deleteResult = false;
            try
            {
                deleteResult = _NoticeRepository.Delete(id);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, exp.Message);
            }
            if (deleteResult)
            {
                return new ReturnResult<bool>(1, deleteResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, deleteResult, "通知信息删除失败！");
            }
        }
        /// <summary>
        /// 发布（审核）通知
        /// </summary>
        public ReturnResult<bool> AuditNotice(Notice notice)
        {
            notice.noticestate = 1;//新增状态
            return this.UpdateNotice(notice);
        }
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="filter">查询关键字</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Notice>> GetNotices(string filter = "",
            int pageIndex = 1, int pageSize = 30)
        {
            Expression<Func<Notice, bool>> expression = x =>
                filter == "" || (filter != "" && x.content.Contains(filter));
            SortInfo<Notice> sort = new SortInfo<Notice>(p => p.publishtime, SortOrder.Descending);
            var res = _NoticeRepository.Find().Where(p => filter == "" || (filter != "" && 
                (p.content.Contains(filter) || p.title.Contains(filter))))
                .OrderByDescending(q => q.publishtime).ToPage(pageIndex, pageSize);
            return new ReturnResult<IPagedData<Notice>>(1, res);
        }
        #endregion
    }
}
