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
using QuickShare.LiteFramework.Foundation;
using QuickShare.LiteFramework;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class NoticeService : BaseService<Notice, Notice>, INoticeService
    {
        #region 构造函数
        private readonly IRepository<Notice> _NoticeRepository;
        private static JPushClient client = new JPushClient("2e6a365d5d05e1097e38339c", "af70172e784fd7209373746d");
        private readonly ILogger logger;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public NoticeService(IRepository<Notice> noticeRepository) :
            base(noticeRepository)
        {
            this._NoticeRepository = noticeRepository;
            logger = AppInstance.Current.Resolve<ILogger>();
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
            notice.updatetime = DateTime.Now;
            bool saveResult = false;
            try
            {
                saveResult = _NoticeRepository.Add(notice);
            }
            catch (Exception exp)
            {
                logger.LogError("新增通知出错！", exp);
                return new ReturnResult<Notice>(-1, "系统异常，请稍后重试。");
            }
            if (saveResult)
            {
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
            notice.updatetime = DateTime.Now;
            try
            {
                updateResult = _NoticeRepository.Update(notice);
            }
            catch (Exception exp)
            {
                logger.LogError("更新通知信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
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
                logger.LogError("删除通知信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
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
        public ReturnResult<bool> AuditNotice(string noticeId)
        {
            var notice = _NoticeRepository.Find().Where(p => p.id == noticeId).FirstOrDefault();
            if(notice == null || string.IsNullOrEmpty(notice.id))
            {
                return new ReturnResult<bool>(-2, "找不到对应的通知！");
            }
            else if(notice.noticestate != 0)
            {
                return new ReturnResult<bool>(-2, "通知当前状态不可审核！");
            }
            notice.noticestate = 1;//新增状态
            notice.updatetime = DateTime.Now;

            var res = this.UpdateNotice(notice);
            if (res.code > 0)
            {
                //消息保存成功，发起推送
                PushPayload pushPayload = new PushPayload()
                {
                    Platform = new List<string> { "android" },
                    Audience = "all",
                    Notification = new Notification
                    {
                        Android = new Android
                        {
                            Alert = notice.content,
                            Title = notice.title,
                        },
                    },
                    Message = new Message
                    {
                        Title = notice.title,
                        Content = notice.content,
                    },
                };
                var response = client.SendPush(pushPayload);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //失败需要记录日志
                    logger.LogInfo(string.Format("极光推送消息失败！通知ID：{0}。错误详情:{1}",
                        notice.id, response.Content));
                }
            }
            return res;
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
            var res = _NoticeRepository.Find().Where(p => ( filter == null ||  filter == "" 
            || (filter != null && filter != "" &&  (p.content.Contains(filter) || p.title.Contains(filter)))) 
            && p.noticestate == 1)
                .OrderByDescending(q => q.publishtime).ToPage(pageIndex, pageSize);
            return new ReturnResult<IPagedData<Notice>>(1, res);
        }
        #endregion
    }
}
