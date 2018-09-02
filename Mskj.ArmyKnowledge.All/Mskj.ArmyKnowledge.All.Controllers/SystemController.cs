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
        private readonly IProductService _ProductService;
        private readonly IDemandService _DemandService;
        private readonly IQuestionService _QuestionService;

        public SystemController(ISystemService systemService, IMsgService msgService,
            INoticeService noticeService, IUsersService usersService, IDemandService demandService,
            IProductService productService, IQuestionService questionService)
        {
            _SystemService = systemService;
            _MsgService = msgService;
            _NoticeService = noticeService;
            _UsersService = usersService;
            _DemandService = demandService;
            _ProductService = productService;
            _QuestionService = questionService;
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

        #region 图片上传
        /// <summary>
        /// 产品图片上传
        /// </summary>
        [Route("UploadImages")]
        [HttpPost]
        public object UploadImages()
        {
            return _SystemService.UploadFile();
        }
        #endregion

        #region 全文检索
        /// <summary>
        /// 获取检索结果
        /// </summary>
        [Route("GetFilterInfos")]
        [HttpGet]
        public object GetFilterInfos(string filter,int pageIndex = 1,int pageSize = 2)
        {
            if(string.IsNullOrEmpty(filter))
            {
                return new ReturnResult<FtrModel>(-4, "请输入搜索内容！");
            }
            FtrModel ftr = new FtrModel();
            //用户
            var user = _UsersService.GetUsers(filter:filter, userType:-1, pageIndex: pageIndex,pageSize: pageSize);
            if (user.code > 0)
            {
                ftr.UserInfo = user.data;
            }
            //提问
            var question = _QuestionService.GetQuestions(filter: filter, pageIndex: pageIndex, pageSize: pageSize);
            if(question.code > 0)
            {
                ftr.QuestionInfo = question.data;
            }
            //产品
            var product = _ProductService.GetProducts(filter: filter, pageIndex: pageIndex, pageSize: pageSize);
            if (product.code > 0)
            {
                ftr.ProductInfo = product.data;
            }
            //需求
            var demand = _DemandService.GetDemands(filter: filter, pageIndex: pageIndex, pageSize: pageSize);
            if (demand.code > 0)
            {
                ftr.DemandInfo = demand.data;
            }
            return new ReturnResult<FtrModel>(1,ftr);
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
        public object GetMsgDetail(string msgId, string filter = "",int pageIndex = 1, int pageSize = 30)
        {
            return _MsgService.GetMsgDetail(msgId,filter,pageIndex,pageSize);
        }
        /// <summary>
        /// 获取用户的消息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        [Route("GetUserMsgs")]
        [HttpGet]
        public object GetUserMsgs(string userid, int pageIndex = 1, int pageSize = 30)
        {
            return _MsgService.GetUserMsgs(userid, pageIndex, pageSize);
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
            if (notice == null || string.IsNullOrEmpty(notice.title) ||
                string.IsNullOrEmpty(notice.content))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
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
        public object AuditNotice(PostId notice)
        {
            if (notice == null || string.IsNullOrEmpty(notice.Id))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _NoticeService.AuditNotice(notice.Id);
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

        #region 字典相关信息
        /// <summary>
        /// 获取欢迎页
        /// </summary>
        [Route("GetWelcomeImage")]
        [HttpGet]
        public object GetWelcomeImage()
        {
            var resTemp = _SystemService.GetDictionarys(3);
            if(resTemp != null && resTemp.Count > 0)
            {
                return new ReturnResult<string>(1, resTemp[0]); 
            }
            return new ReturnResult<string>(-1,"未设置欢迎页面！");
        }
        /// <summary>
        /// 获取轮播图
        /// </summary>
        [Route("GetCarouselImages")]
        [HttpGet]
        public object GetCarouselImages()
        {
            var resTemp = _SystemService.GetDictionarys(4);
            if (resTemp != null && resTemp.Count > 0)
            {
                return new ReturnResult<List<string>>(1, resTemp);
            }
            return new ReturnResult<List<string>>(-1, "未设置轮播图片！");
        }
        /// <summary>
        /// 增加字典
        /// </summary>
        [Route("AddDictionary")]
        [HttpPost]
        public object AddDictionary(Dictionary dic)
        {
            if(dic == null || string.IsNullOrEmpty(dic.dicname))
            {
                return new ReturnResult<Dictionary>(-4, "传入参数错误!");
            }
            return _SystemService.AddDictionary(dic);
        }
        /// <summary>
        /// 更新字典
        /// </summary>
        [Route("UpdateDictionary")]
        [HttpPost]
        public object UpdateDictionary(Dictionary dic)
        {
            if (dic == null || string.IsNullOrEmpty(dic.dicname))
            {
                return new ReturnResult<Dictionary>(-4, "传入参数错误!");
            }
            return _SystemService.UpdateDictionary(dic);
        }
        /// <summary>
        /// 删除字典
        /// </summary>
        [Route("DeleteDictionary")]
        [HttpPost]
        public object DeleteDictionary(PostId dic)
        {
            if (dic == null || string.IsNullOrEmpty(dic.Id))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _SystemService.DeleteDictionary(dic.Id);
        }
        /// <summary>
        /// 获取字典列表
        /// </summary>
        [Route("GetDictionarys")]
        [HttpGet]
        public object GetDictionarys(int dictype = -1,int pageIndex = 1, int pageSize = 30)
        {
            return _SystemService.GetDictionarys(dictype,pageIndex,pageSize);
        }
        #endregion
    }
}
