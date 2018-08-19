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

namespace Mskj.ArmyKnowledge.All.Services
{
    public class MsgService : BaseService<Msg, Msg>, IMsgService
    {
        #region 构造函数
        private readonly IRepository<Msg> _MsgRepository;
        private readonly IRepository<MsgDetail> _MsgDetailRepository;
        private readonly IRepository<Notice> _NoticeRepository;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public MsgService(IRepository<Msg> msgRepository,
            IRepository<MsgDetail> msgDetailRepository, IRepository<Notice> noticeRepository) :
            base(msgRepository)
        {
            this._MsgRepository = msgRepository;
            this._MsgDetailRepository = msgDetailRepository;
            this._NoticeRepository = noticeRepository;
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
            msg.id = Guid.NewGuid().ToString();
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
        public ReturnResult<List<MsgDetail>> AddMsgDetail(List<MsgDetail> msgDetails)
        {
            if (msgDetails == null || msgDetails.Count <= 0)
            {
                return new ReturnResult<List<MsgDetail>>(-2, "没有要保存的消息明细数据！");
            }
            int addCount = 0;
            msgDetails.ForEach(p => {
                p.id = Guid.NewGuid().ToString();
                p.sendtime = DateTime.Now;
            });
            try
            {
                addCount = _MsgDetailRepository.Add(msgDetails);
            }
            catch (Exception exp)
            {
                return new ReturnResult<List<MsgDetail>>(-1, exp.Message);
            }
            if (addCount > 0)
            {
                return new ReturnResult<List<MsgDetail>>(1, msgDetails, string.Format("已经保存{0}消息!", addCount));
            }
            else
            {
                return new ReturnResult<List<MsgDetail>>(-2, "消息信息保存失败！");
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
        /// 获取问题列表
        /// </summary>
        /// <param name="filter">查询关键字</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<MsgDetail>> GetQuestions(string filter = "",
            int pageIndex = 1, int pageSize = 30)
        {
            Expression<Func<MsgDetail, bool>> expression = x =>
                filter == "" || (filter != "" && x.content.Contains(filter));
            SortInfo<MsgDetail> sort = new SortInfo<MsgDetail>(p => p.sendtime, SortOrder.Descending);
            var res = _MsgDetailRepository.Find().Where(p => filter == "" || (filter != "" && p.content.Contains(filter)))
                .OrderByDescending(q => q.sendtime).ToPage(pageIndex, pageSize);
            return new ReturnResult<IPagedData<MsgDetail>>(1, res);
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
