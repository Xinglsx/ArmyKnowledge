using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.All.Services;
using Mskj.ArmyKnowledge.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mskj.ArmyKnowledge.All.Controllers
{
    [RoutePrefix("System")]
    public class SystemController : BaseController
    {
        #region 构造函数
        private readonly ISystemService _SystemService;
        private readonly IMsgService _MsgService;
        private readonly INoticeService _NoticeService;

        public SystemController(ISystemService systemService, IMsgService msgService,
            INoticeService noticeService)
        {
            _SystemService = systemService;
            _MsgService = msgService;
            _NoticeService = noticeService;
        }
        #endregion

        #region 版本信息
        /// <summary>
        /// 获取版本信息
        /// </summary>
        [Route("GetVersionInfo")]
        [HttpGet]
        public object GetVersionInfo()
        {
            return _SystemService.GetVersionInfo();
        }
        #endregion

        #region 消息信息
        /// <summary>
        /// 新增消息会话
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Route("AddMsg")]
        [HttpPost]
        public object AddMsg(Msg msg)
        {
            return _MsgService.AddMsg(msg);
        }
        /// <summary>
        /// 新增消息明细列表
        /// </summary>
        /// <param name="msgDetails"></param>
        /// <returns></returns>
        [Route("AddMsgDetail")]
        [HttpPost]
        public object AddMsgDetail(MsgDetail msgDetails)
        {
            return _MsgService.AddMsgDetail(msgDetails);
        }
        /// <summary>
        /// 删除消息会话
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        [Route("DeleteMsg")]
        [HttpPost]
        public object DeleteMsg(string msgId)
        {
            return _MsgService.DeleteMsg(msgId);
        }
        /// <summary>
        /// 删除消息明细列表
        /// </summary>
        /// <param name="msgDetailIds"></param>
        /// <returns></returns>
        [Route("DeleteMsgDetails")]
        [HttpPost]
        public object DeleteMsgDetails(List<string> msgDetailIds)
        {
            return _MsgService.DeleteMsgDetails(msgDetailIds);
        }
        /// <summary>
        /// 获取消息明细列表
        /// </summary>
        /// <param name="filter">查询关键字</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        [Route("GetMsgDetail")]
        [HttpGet]
        public object GetMsgDetail(string filter = "",int pageIndex = 1, int pageSize = 30)
        {
            return _MsgService.GetMsgDetail(filter,pageIndex,pageSize);
        }
        #endregion

        #region 通知信息
        /// <summary>
        /// 新增通知
        /// </summary>
        [Route("AddNotice")]
        [HttpPost]
        public object AddNotice(Notice notice)
        {
            return _NoticeService.AddNotice(notice);
        }
        /// <summary>
        /// 更新通知信息
        /// </summary>
        /// <param name="notice">新的通知信息</param>
        [Route("UpdateNotice")]
        [HttpPost]
        public object UpdateNotice(Notice notice)
        {
            return _NoticeService.UpdateNotice(notice);
        }
        /// <summary>
        /// 删除通知信息
        /// </summary>
        /// <param name="id">通知ID</param>
        [Route("DeleteNotice")]
        [HttpPost]
        public object DeleteNotice(string id)
        {
            return _NoticeService.DeleteNotice(id);
        }
        /// <summary>
        /// 发布（审核）通知
        /// </summary>
        [Route("AuditNotice")]
        [HttpPost]
        public object AuditNotice(Notice notice)
        {
            return _NoticeService.AuditNotice(notice);
        }
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="filter">查询关键字</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        [Route("GetNotices")]
        [HttpGet]
        public object GetNotices(string filter = "",int pageIndex = 1, int pageSize = 10)
        {
            return _NoticeService.GetNotices(filter, pageIndex, pageSize);
        }
        #endregion
    }
}
