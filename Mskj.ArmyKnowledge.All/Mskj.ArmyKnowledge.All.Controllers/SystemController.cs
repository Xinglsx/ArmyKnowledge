using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common;
using System.Collections.Generic;
using System.Web.Http;
using System.Web;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Common.DataObj;

namespace Mskj.ArmyKnowledge.All.Controllers
{
    [RoutePrefix("System")]
    public class SystemController : BaseController
    {
        #region 构造函数
        private readonly ISystemService _SystemService;
        private readonly IMsgService _MsgService;
        private readonly INoticeService _NoticeService;
        private readonly IUsersService _UsersService;

        public SystemController(ISystemService systemService, IMsgService msgService,
            INoticeService noticeService, IUsersService usersService)
        {
            _SystemService = systemService;
            _MsgService = msgService;
            _NoticeService = noticeService;
            _UsersService = usersService;
        }
        #endregion

        #region 版本信息
        /// <summary>
        /// 获取版本信息
        /// </summary>
        [Route("GetVersionInfo")]
        [HttpPost]
        public object GetVersionInfo()
        {
            return _SystemService.GetVersionInfo();
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        [Route("UploadFile")]
        [HttpGet]
        public object UploadFile()
        {
            HttpContext context = HttpContext.Current;
            string fileName = @"D:\abc.jpg";
            //保存文件，此处跟普通MVC一样
            context.Request.Files[0].SaveAs(fileName);
            return new ReturnResult<string>(1,"文件地址","上传成功");
        }
        #endregion

        #region 阿里云消息
        /// <summary>
        /// 新增用户时，发送手机短信
        /// </summary>
        [Route("SendMobileMessage")]
        [HttpPost]
        public object AddUserSendMessage(PostUser phone)
        {
            var isExist = _UsersService.ExistsUserByPhoneNumber(phone.PhoneNumber);
            if (isExist)
            {
                return new ReturnResult<bool>(-2, "手机号已经被使用，请更换！");
            }
            return _SystemService.SendMobileMessage(phone.PhoneNumber);
        }
        /// <summary>
        /// 修改密码时，发送手机短信
        /// </summary>
        [Route("ChangePwdSendMessage")]
        [HttpPost]
        public object ChangePwdSendMessage(PostUser phone)
        {
            if (phone == null)
            {
                return new ReturnResult<bool>(-2, "参数传入错误");
            }
            var isExist = _UsersService.ExistsUserByPhoneNumber(phone.PhoneNumber);
            if (!isExist)
            {
                return new ReturnResult<bool>(-2, "手机号未注册！");
            }
            return _SystemService.SendMobileMessage(phone.PhoneNumber);
        }
        /// <summary>
        /// 验证手机号
        /// </summary>
        [Route("ValidationCode")]
        [HttpPost]
        public object ValidationCode(PostUser phoneCode)
        {
            if (phoneCode == null)
            {
                return new ReturnResult<bool>(-2, "参数传入错误");
            }
            return _SystemService.ValidationCode(phoneCode.PhoneNumber, phoneCode.VerificationCode);
        }
        #endregion

        #region 消息信息
        /// <summary>
        /// 新增消息会话
        /// </summary>
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
