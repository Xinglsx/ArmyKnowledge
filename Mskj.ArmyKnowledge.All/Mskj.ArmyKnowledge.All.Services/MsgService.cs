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
using Jiguang.JPush;
using Jiguang.JPush.Model;
using QuickShare.LiteFramework;
using QuickShare.LiteFramework.Foundation;
using Mskj.ArmyKnowledge.All.Common.DataObj;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class MsgService : BaseService<Msg, Msg>, IMsgService
    {
        #region 构造函数
        private readonly IRepository<Msg> _MsgRepository;
        private readonly IRepository<MsgDetail> _MsgDetailRepository;
        private readonly IRepository<Notice> _NoticeRepository;
        private readonly IRepository<Users> _UserRepository;
        private static JPushClient client = new JPushClient("2e6a365d5d05e1097e38339c", "af70172e784fd7209373746d");
        private readonly ILogger logger;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public MsgService(IRepository<Msg> msgRepository,
            IRepository<MsgDetail> msgDetailRepository, IRepository<Notice> noticeRepository,
            IRepository<Users> userRepository) :
            base(msgRepository)
        {
            this._MsgRepository = msgRepository;
            this._MsgDetailRepository = msgDetailRepository;
            this._NoticeRepository = noticeRepository;
            this._UserRepository = userRepository;

            logger = AppInstance.Current.Resolve<ILogger>();
        }
        #endregion

        #region 消息信息
        /// <summary>
        /// 新增消息会话
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ReturnResult<Msg> AddMsg(Msg msg)
        {
            var existMsg = GetOne(p => (p.userid1 == msg.userid1 && p.userid2 == msg.userid2)
                || (p.userid1 == msg.userid2 && p.userid2 == msg.userid1));
            if(existMsg != null && !string.IsNullOrEmpty(existMsg.id))
            {
                return new ReturnResult<Msg>(1, existMsg, "已存在消息会话!");
            }

            msg.id = Guid.NewGuid().ToString();
            msg.updatetime = DateTime.Now;
            bool saveResult = false;
            try
            {
                saveResult = this.Add(msg);
            }
            catch (Exception exp)
            {
                logger.LogError("新增消息会话出错！", exp);
                return new ReturnResult<Msg>(-1, "系统异常，请稍后重试。");
            }
            if (saveResult)
            {
                return new ReturnResult<Msg>(1, msg);
            }
            else
            {
                return new ReturnResult<Msg>(-2, "通知信息保存失败！");
            }
        }
        /// <summary>
        /// 新增消息明细列表
        /// </summary>
        /// <param name="msgDetails"></param>
        /// <returns></returns>
        public ReturnResult<MsgDetail> AddMsgDetail(MsgDetail msgDetail)
        {
            bool addRes = false;
            msgDetail.id = Guid.NewGuid().ToString();
            msgDetail.sendtime = DateTime.Now;
            try
            {
                addRes = _MsgDetailRepository.Add(msgDetail);
            }
            catch (Exception exp)
            {
                return new ReturnResult<MsgDetail>(-1, exp.Message);
            }
            if (addRes)
            {
                var msg = GetOne(p => p.id == msgDetail.msgid);
                if(msg != null && !string.IsNullOrEmpty(msg.id))
                {
                    msg.lastcontent = msgDetail.content;
                    msg.updatetime = DateTime.Now;
                    try
                    {
                        Update(msg);

                    }
                    catch (Exception exp)
                    {
                        logger.LogError("新增产品信息出错！", exp);
                    }
                }
                string acceptUserid = msg.userid1 == msgDetail.senduserid ? msg.userid2 :
                    msg.userid1;
                if (!string.IsNullOrEmpty(acceptUserid))
                {
                    string phone = _UserRepository.Find().Where(p => p.id == acceptUserid)
                        .Select(q => q.phonenumber).FirstOrDefault();
                    //消息保存成功，发起推送
                    PushPayload pushPayload = new PushPayload()
                    {
                        Platform = new List<string> { "android" },
                        Audience = new Audience
                        {
                            Alias = new List<string>{
                                phone,
                            },
                        },
                        Notification = new Notification
                        {
                            Android = new Android
                            {
                                Alert = msgDetail.content,
                                Title = msgDetail.sendnickname + "发给您一条私信！"
                            }
                        },
                        Message = new Message
                        {
                            Title = "私信",
                            Content = msgDetail.content,
                        },
                        
                    };
                    var response = client.SendPush(pushPayload);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        //失败需要记录日志
                        logger.LogInfo(string.Format("极光推送消息失败！消息会话ID：{0}。错误详情:{1}",
                            msg.id, response.Content));
                    }

                    //response.Content "{\"error\":{\"code\":1003,\"message\":\"The alias has invalid character\"}}"
                    //"{\"sendno\":\"0\",\"msg_id\":\"1073574972\"}"
                }
                //每发一条私信，增加1分。
                var users = _UserRepository.Find().Where(p => p.id == msg.userid1 || p.id == msg.userid2).ToList();
                if (users != null && users.Count() > 0)
                {
                    users.ForEach(p => p.compositescores++);
                    _UserRepository.Update(users);
                }
                return new ReturnResult<MsgDetail>(1, msgDetail);
            }
            else
            {
                return new ReturnResult<MsgDetail>(-2, "消息信息保存失败！");
            }
        }
        /// <summary>
        /// 删除消息会话
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public ReturnResult<bool> DeleteMsg(string msgId)
        {
            bool deleteRes = false;
            try
            {
                var msgDetails = _MsgDetailRepository.Find().Where(p => p.msgid == msgId);
                if (msgDetails != null && msgDetails.Count() > 0)
                {
                    _MsgDetailRepository.Delete(msgDetails);
                }
                deleteRes = this.DeleteByKey(msgId);
            }
            catch (Exception exp)
            {
                logger.LogError("删除消息会话出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (deleteRes)
            {
                return new ReturnResult<bool>(1, true);
            }
            else
            {
                return new ReturnResult<bool>(-2, "消息信息删除失败！");
            }
        }
        /// <summary>
        /// 删除消息明细列表
        /// </summary>
        /// <param name="msgDetailIds"></param>
        /// <returns></returns>
        public ReturnResult<bool> DeleteMsgDetails(List<string> msgDetailIds)
        {
            bool deleteRes = false;
            try
            {
                deleteRes = _MsgDetailRepository.Delete(msgDetailIds);
            }
            catch (Exception exp)
            {
                logger.LogError("删除消息明细列表！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (deleteRes)
            {
                return new ReturnResult<bool>(1, true);
            }
            else
            {
                return new ReturnResult<bool>(-2, "消息信息删除失败！");
            }
        }
        /// <summary>
        /// 获取消息明细信息
        /// </summary>
        /// <param name="filter">查询关键字</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<MsgDetail>> GetMsgDetail(string msgId, string filter = "",
            int pageIndex = 1, int pageSize = 30)
        {
            var res = _MsgDetailRepository.Find().Where(p => p.msgid == msgId && 
                (filter == null || filter == "" || (filter != null && filter != "" && p.content.Contains(filter))))
                .OrderByDescending(q => q.sendtime).ToPage(pageIndex, pageSize);
            return new ReturnResult<IPagedData<MsgDetail>>(1, res);
        }
        /// <summary>
        /// 获取用户的消息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<MsgModel>> GetUserMsgs(string userid,int pageIndex = 1,int pageSize = 30)
        {
            var res = (from msg in _MsgRepository.Find()
                       join user1 in _UserRepository.Find() on msg.userid1 equals user1.id into temp1
                       from tempUser1 in temp1.DefaultIfEmpty()
                       join user2 in _UserRepository.Find() on msg.userid2 equals user2.id into temp2
                       from tempUser2 in temp2.DefaultIfEmpty()
                       where msg.userid1 == userid || msg.userid2 == userid
                       orderby msg.updatetime descending
                       select new MsgModel
                       {
                           Id = msg.id,
                           Userid1 = msg.userid1,
                           Nickname1 = tempUser1.nickname,
                           Avatar1 = tempUser1.avatar,
                           Userid2 = msg.userid2,
                           Nickname2 = tempUser2.nickname,
                           Avatar2 = tempUser2.avatar,
                           LastContent = msg.lastcontent,
                           Updatetime = msg.updatetime,
                       }
                       ).ToPage(pageIndex, pageSize);
            if(res == null)
            {

            }
            return new ReturnResult<IPagedData<MsgModel>>(1, res);
        }
        #endregion
        
    }
}
