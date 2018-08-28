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
                return new ReturnResult<Msg>(-1, exp.Message);
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
                    msg.updatetime = DateTime.Now;
                    Update(msg);
                }
                string acccptUserid = msg.userid1 == msgDetail.senduserid ? msg.userid2 :
                    msg.userid1;
                if (string.IsNullOrEmpty(acccptUserid))
                {
                    //消息保存成功，发起推送
                    PushPayload pushPayload = new PushPayload()
                    {
                        Platform = new List<string> { "android" },
                        Audience = new Audience
                        {
                            Alias = new List<string>{
                                acccptUserid,
                            },
                        },
                        Message = new Message
                        {
                            Title = msgDetail.sendnickname + "发给您一条私信！",
                            Content = msgDetail.content,
                        },
                    };
                    var response = client.SendPush(pushPayload);
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
                var msgDetails = _MsgDetailRepository.Find().Where(
                p => p.msgid == msgId);
                if (msgDetails != null && msgDetails.Count() > 0)
                {
                    _MsgDetailRepository.Delete(msgDetails);
                }
                deleteRes = this.DeleteByKey(msgId);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, exp.Message);
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
                return new ReturnResult<bool>(-1, exp.Message);
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
        public ReturnResult<IPagedData<Msg>> GetUserMsgs(string userid,int pageIndex = 1,int pageSize = 30)
        {
            Expression<Func<Msg, bool>> expression = x => x.userid1 == userid || x.userid2 == userid;
            SortInfo<Msg> sort = new SortInfo<Msg>(p => new { p.updatetime }, SortOrder.Descending);
            var res = GetPage(pageIndex, pageSize, sort, expression);
            return new ReturnResult<IPagedData<Msg>>(1, res);
        }
        #endregion
        
    }
}
