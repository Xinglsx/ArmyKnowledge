using Mskj.ArmyKnowledge.All.ServiceContracts;
using QuickShare.LiteFramework.Base;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using System;
using QuickShare.LiteFramework.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Linq;
using QuickShare.LiteFramework.Common.Extenstions;
using System.Collections.Generic;
using QuickShare.LiteFramework.Mapper;
using System.Threading.Tasks;
using Mskj.ArmyKnowledge.All.Common.DataObj;
using System.Web;
using System.Configuration;
using System.IO;
using QuickShare.LiteFramework.Foundation;
using QuickShare.LiteFramework;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class QuestionService : BaseService<QuestionModel, Question>, IQuestionService
    {
        #region 构造函数
        private readonly IRepository<Question> _QuestionRepository;
        private readonly IRepository<Answer> _AnswerDetailRepository;
        private readonly IRepository<Record> _RecordRepository;
        private readonly IRepository<Users> _UserRepository;
        private readonly ILogger logger;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        public QuestionService(IRepository<Question> questionRepository,
            IRepository<Answer> answerDetailRepository, IRepository<Users> userRepository,
            IRepository<Record> recordRepository) :
            base(questionRepository)
        {
            this._QuestionRepository = questionRepository;
            this._AnswerDetailRepository = answerDetailRepository;
            this._UserRepository = userRepository;
            this._RecordRepository = recordRepository;

            logger = AppInstance.Current.Resolve<ILogger>();
        }

        #endregion

        #region 问题回答
        /// <summary>
        /// 新增问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public ReturnResult<QuestionModel> AddQuestion(QuestionModel question)
        {
            question.Id = Guid.NewGuid().ToString();
            question.HeatCount = 1000;//初始分为1000
            question.Publishtime = DateTime.Now;
            bool res = false;
            try
            {
                res = this.Add(question);
            }
            catch (Exception exp)
            {
                logger.LogError("新增问题出错！", exp);
                return new ReturnResult<QuestionModel>(-1, "系统异常，请稍后重试。");
            }
            if(res)
            {
                var user = _UserRepository.Find().Where(p => p.id == question.Author).FirstOrDefault();
                if(user != null)
                {
                    user.compositescores += 1;//提个问题+1分
                    _UserRepository.Update(user);
                }
                return new ReturnResult<QuestionModel>(1, question);
            }
            else
            {
                return new ReturnResult<QuestionModel>(-2, "问题数据保存失败！");
            }
        }
        /// <summary>
        /// 更新问题
        /// </summary>
        public ReturnResult<bool> UpdateQuestion(QuestionModel question)
        {
            bool res = false;
            try
            {
                res = Update(question);
            }
            catch (Exception exp)
            {
                logger.LogError("更新问题出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (res)
            {
                UpdateHeatCount(question);
                return new ReturnResult<bool>(1, res);
            }
            else
            {
                return new ReturnResult<bool>(-2, res, "用户信息更新失败！");
            }
        }
        /// <summary>
        /// 提交审核问题
        /// </summary>
        public ReturnResult<bool> SubmitQuestion(string id)
        {
            QuestionModel question = GetOne(p => p.Id == id);
            if(question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<bool>(-2, "未找到对应提问！");
            }
            if (question.QuestionState != 0 && question.QuestionState != 3)
            {
                return new ReturnResult<bool>(-2, "提问状态不是[新建状态]或[审核不通过状态]！");
            }
            question.QuestionState = 1;
            return this.UpdateQuestion(question);
        }
        /// <summary>
        /// 审核通过问题
        /// </summary>
        public ReturnResult<bool> AuditQuestion(string id)
        {
            QuestionModel question = GetOne(p => p.Id == id);
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<bool>(-2, "未找到对应提问！");
            }
            if (question.QuestionState != 1)
            {
                return new ReturnResult<bool>(-2, "提问状态不是[提交审核状态]！");
            }
            question.QuestionState = 2;
            var res = UpdateQuestion(question);
            if(res.code > 0)
            {
                var user = _UserRepository.Find().Where(p => p.id == question.Author).FirstOrDefault();
                if (user != null)
                {
                    user.compositescores += 4;//审核通过问题+4分
                    _UserRepository.Update(user);
                }
            }
            return res;
        }
        /// <summary>
        /// 审核不通过问题
        /// </summary>
        public ReturnResult<bool> AuditFailQuestion(string id)
        {
            QuestionModel question = GetOne(p => p.Id == id);
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<bool>(-2, "未找到对应提问！");
            }
            if (question.QuestionState != 1)
            {
                return new ReturnResult<bool>(-2, "提问状态不是[提交审核状态]！");
            }
            question.QuestionState = 3;
            var res = UpdateQuestion(question);
            return res;
        }
        /// <summary>
        /// 删除问题
        /// </summary>
        public ReturnResult<bool> DeleteQuestion(string id)
        {
            bool deleteResult = false;
            try
            {
                deleteResult = this.DeleteByKey(id);
            }
            catch (Exception exp)
            {
                logger.LogError("删除问题出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (deleteResult)
            {
                return new ReturnResult<bool>(1, deleteResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, deleteResult, "用户信息删除失败！");
            }
        }
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="state">状态 -1 全部 </param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sortType">排序方式：0-时间倒序 1-热值倒序 2-评论倒序 </param>
        /// <param name="filter">查询关键字</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<QuestionModel>> GetQuestions(string userid = "",string filter = "",
            int state = 2,int pageIndex = 1,int pageSize = 10, int sortType = 0)
        {
            var res = (from question in _QuestionRepository.Find()
                       join user in _UserRepository.Find() on question.author equals user.id
                       join record in _RecordRepository.Find() on new { question.id, userid }
                       equals new { id = record.questionid, record.userid } into temp
                       from tempRecord in temp.DefaultIfEmpty()
                       select new QuestionModel
                       {
                           Author = user.id,
                           AuthorNickname = user.nickname,
                           Avatar = user.avatar,
                           CommentCount = question.commentcount,
                           Content = question.content,
                           HeatCount = question.heatcount,
                           HomeImage = question.homeimage,
                           Id = question.id,
                           Images = question.images,
                           Introduction = question.introduction,
                           IsCollect = tempRecord.iscollect,
                           IsPraise = tempRecord.ispraise,
                           IsRecommend = question.isrecommend,
                           PraiseCount = question.praisecount,
                           Publishtime = question.publishtime,
                           QuestionState = question.questionstate,
                           ReadCount = question.readcount,
                           Title = question.title,
                       });
            if(state != -1)
            {
                res = res.Where(x => x.QuestionState == state);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                res = res.Where(x => x.Title.Contains(filter) || x.AuthorNickname.Contains(filter) ||
                    x.Introduction.Contains(filter) || x.Content.Contains(filter)
                    );

            }
            switch (sortType)
            {
                case 1:
                    res = res.OrderByDescending(p => p.HeatCount).ThenByDescending(p => p.Publishtime);
                    break;
                case 2:
                    res = res.OrderByDescending(p => p.CommentCount).ThenByDescending(p => p.Publishtime);
                    break;
                case 0:
                default:
                    res = res.OrderByDescending(p => p.Publishtime);
                    break;
            }
            var result = res.ToPage(pageIndex, pageSize);

            return new ReturnResult<IPagedData<QuestionModel>>(1, result);
        }
        /// <summary>
        /// 分页获取用户对应的问题列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<QuestionModel>> GetUserQuestion(string userid,
            string filter = "",int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            Expression<Func<QuestionModel, bool>> expression;
            if (string.IsNullOrEmpty(filter))
            {
                expression = x => x.Author == userid;
            }
            else
            {
                expression = x => x.Author == userid && (
                    x.Title.Contains(filter) || x.AuthorNickname.Contains(filter) ||
                    x.Introduction.Contains(filter) || x.Content.Contains(filter)
                    );

            }
            return GetBaseQuestionModels(pageIndex, pageSize, sortType, expression);
        }
        /// <summary>
        /// 分页获取问题列表(封装排序方式)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式：0-时间倒序 1-热值倒序 2-评论倒序 </param>
        /// <param name="expression">查询表达示</param>
        /// <returns></returns>
        private ReturnResult<IPagedData<QuestionModel>> GetBaseQuestionModels(int pageIndex,
            int pageSize, int sortType, Expression<Func<QuestionModel, bool>> expression)
        {
            List<SortInfo<QuestionModel>> sorts = new List<SortInfo<QuestionModel>>();
            SortInfo<QuestionModel> sortTime = new SortInfo<QuestionModel>(p => new { p.Publishtime },
                        SortOrder.Descending);
            switch (sortType)
            {
                case 1:
                    sorts.Add(new SortInfo<QuestionModel>(p => new { p.HeatCount },
                        SortOrder.Descending));
                    sorts.Add(sortTime);
                    break;
                case 2:
                    sorts.Add(new SortInfo<QuestionModel>(p => new { p.CommentCount },
                        SortOrder.Descending));
                    sorts.Add(sortTime);
                    break;
                case 0:
                default:
                    sorts.Add(sortTime);
                    break;
            }
            return new ReturnResult<IPagedData<QuestionModel>>(1,
                    GetPage(pageIndex, pageSize, sorts, expression));
        }
        /// <summary>
        /// 分页获取问题的回答
        /// </summary>
        /// <param name="questionId">问题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<AnswerModel>> GetAnswers( string questionId,
            int pageIndex = 1,int pageSize = 10)
        {
            var res = (from answer in _AnswerDetailRepository.Find()
                       join user in _UserRepository.Find() on answer.userid equals user.id
                       where answer.questionid == questionId
                       select new AnswerModel
                       {
                           Userid = user.id,
                           Nickname = user.nickname,
                           Avatar = user.avatar,
                           Content = answer.content,
                           Id = answer.id,
                           Images = answer.images,
                           Publishtime = answer.publishtime,
                           Isadopt = answer.isadopt,
                           Praisecount = answer.praisecount,
                           Questionid = answer.questionid,
                       }).OrderByDescending(p => p.Publishtime).ToPage(pageIndex,pageSize);
            return new ReturnResult<IPagedData<AnswerModel>>(1, res);
        }
        /// <summary>
        /// 分页获取对应用户回答的问题
        /// </summary>
        /// <param name="questionId">问题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Question>> GetUserAnswers(string userid,
            int pageIndex = 1, int pageSize = 10)
        {
            var res = (from answer in _AnswerDetailRepository.Find()
                       join question in _QuestionRepository.Find() on answer.questionid equals question.id
                       where answer.userid == userid
                       orderby answer.publishtime descending
                       select question).ToPage(pageIndex, pageSize);
            return new ReturnResult<IPagedData<Question>>(1, res);
        }
        /// <summary>
        /// 获取一个问题
        /// </summary>
        public ReturnResult<QuestionModel> GetOneQuestion(string questionId,
            int pageIndex= 1, int pageSize = 10)
        {
            QuestionModel question = GetOne(p => p.Id == questionId);
            if(question != null && !string.IsNullOrEmpty(question.Id))
            {
                question.Avatar = _UserRepository.Find().Where(p => p.id == question.Author)
                .Select(q => q.avatar).FirstOrDefault();
                var answers = GetAnswers(questionId, pageIndex, pageSize);
                question.Answers =  answers.data;
                return new ReturnResult<QuestionModel>(1, question);
            }
            else
            {
                return new ReturnResult<QuestionModel>(-2, "对应问题不存在!");
            }
        }
        /// <summary>
        /// 增加阅读数
        /// </summary>
        public ReturnResult<bool> UpdateReadCount(string questionId)
        {
            QuestionModel question = this.GetOne(p => p.Id == questionId);
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<bool>(-2, "问题ID不存在！");
            }
            else
            {
                question.ReadCount++;
                var res = UpdateQuestion(question);
                if(res.code > 0)
                {
                    UpdateHeatCount(question);
                }
                return res;
            }
        }
        /// <summary>
        /// 增加点击数
        /// </summary>
        public ReturnResult<bool> UpdatePraiseCount(string questionId)
        {
            QuestionModel question = this.GetOne(p => p.Id == questionId);
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<bool>(-2, "问题ID不存在！");
            }
            else
            {
                question.PraiseCount++;
                var res = UpdateQuestion(question);
                if(res.code > 0)
                {
                    UpdateHeatCount(question);
                }
                return res;
            }
        }
        /// <summary>
        /// 增加评论数
        /// </summary>
        public ReturnResult<bool> UpdateCommentCount(string questionId)
        {
            QuestionModel question = this.GetOne(p => p.Id == questionId);
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<bool>(-2, "问题ID不存在！");
            }
            else
            {
                question.CommentCount++;
                var res = UpdateQuestion(question);
                return res;
            }
        }
        /// <summary>
        /// 更新热度
        /// </summary>
        public void UpdateHeatCount(string questionId)
        {
            var question = GetOne(p => p.Id == questionId);
            if(question == null || string.IsNullOrEmpty(question.Id))
            {
                return;
            }
            this.UpdateHeatCount(question);
        }
        /// <summary>
        /// 更新热度
        /// </summary>
        public void UpdateHeatCount(QuestionModel question)
        {
            question.HeatCount = 1000 + question.ReadCount;
            question.HeatCount += question.PraiseCount * 5;
            question.HeatCount += question.CommentCount * 10;
            //距离现在的时间越长，减分也越大。
            int hours = (DateTime.Now - question.Publishtime.Value).Hours;
            //当天的，每过一个小时减5
            if (hours <= 24)
            {
                question.HeatCount -= hours * 5;
            }
            //2~5天的，每过一个小时减10
            else if (hours <= 120 && hours > 24)
            {
                question.HeatCount -= 120 + (hours - 24) * 10;
            }
            //5天以上的，每过一个小时减100
            else
            {
                question.HeatCount -= 1080 + (hours - 120) * 100;
            }

            this.Update(question);
        }
        /// <summary>
        /// 增加评论
        /// </summary>
        public ReturnResult<bool> AddAnswer(Answer answer)
        {
            answer.id = Guid.NewGuid().ToString();
            bool res = false;
            try
            {
                res = _AnswerDetailRepository.Add(answer);
            }
            catch (Exception exp)
            {
                logger.LogError("AddAnswer增加评论出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (res)
            {
                //评论增加成功时，更新主表。
                UpdateCommentCount(answer.questionid);
                var user = _UserRepository.Find().Where(p => p.id == answer.userid).FirstOrDefault();
                if (user != null)
                {
                    user.compositescores += 2;//回答一个问题+2分
                    try
                    {
                        _UserRepository.Update(user);
                    }
                    catch (Exception exp)
                    {
                        logger.LogError("AddAnswer增加评论时更新用户综合得分出错！", exp);
                    }
                }
                return new ReturnResult<bool>(1, res);
            }
            else
            {
                return new ReturnResult<bool>(-2, "问题数据保存失败！");
            }
        }
        #endregion

        #region 最近浏览
        /// <summary>
        /// 增加或更新最近浏览
        /// </summary>
        /// <returns></returns>
        public ReturnResult<bool> AddRecord(Record record)
        {
            if(string.IsNullOrEmpty(record.questionid))
            {
                return new ReturnResult<bool>(-2, "未找到待保存的问题ID！");
            }
            if (string.IsNullOrEmpty(record.userid))
            {
                return new ReturnResult<bool>(-2, "未找到待保存的用户ID！");
            }
            var existRecord = _RecordRepository.Find().Where(p => p.questionid == record.questionid
                && p.userid == record.userid).FirstOrDefault();
            bool res = false;
            if(existRecord == null || string.IsNullOrEmpty(existRecord.id))
            {
                record.id = Guid.NewGuid().ToString();
                record.lasttime = DateTime.Now;
                try
                {
                    res = _RecordRepository.Add(record);
                }
                catch (Exception exp)
                {
                    logger.LogError("AddRecord增加最近浏览出错！", exp);
                    return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
                }
            }
            else
            {
                existRecord.lasttime = DateTime.Now;
                existRecord.iscollect = record.iscollect;
                try
                {
                    res = _RecordRepository.Update(existRecord);
                }
                catch (Exception exp)
                {
                    logger.LogError("AddRecord更新最近浏览出错！", exp);
                    return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
                }
            }
            if (res)
            {
                return new ReturnResult<bool>(1, res);
            }
            else
            {
                return new ReturnResult<bool>(-2, "更新最近浏览失败！");
            }
        }
        /// <summary>
        /// 查看最近浏览的问题列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<QuestionModel>> GetRecordQuestions(string userid,
            int pageIndex = 1, int pageSize = 10, string filter= "")
        {
            var res = (from question in _QuestionRepository.Find()
                       join record in _RecordRepository.Find() on question.id equals record.questionid
                       where record.userid == userid
                       orderby record.lasttime descending
                       select question).ToPage(pageIndex,pageSize).
                       MapTo<Question,QuestionModel>();
            return new ReturnResult<IPagedData<QuestionModel>>(1, res);
        }
        /// <summary>
        /// 增加最近浏览
        /// </summary>
        public ReturnResult<bool> AddCollect(Record record)
        {
            record.iscollect = true;
            return this.AddRecord(record);
        }
        /// <summary>
        /// 查看我收藏的问题列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<QuestionModel>> GetCollectionQuestions(string userid,
            int pageIndex = 1, int pageSize = 10, string filter = "")
        {
            var res = (from question in _QuestionRepository.Find()
                       join record in _RecordRepository.Find() on question.id equals record.questionid
                       where record.userid == userid && record.iscollect &&
                       (filter == "" || 
                       (question.title.Contains(filter) || question.content.Contains(filter) ||
                       question.introduction.Contains(filter) || question.author.Contains(filter)))
                       orderby record.lasttime descending
                       select question).ToPage(pageIndex, pageSize).
                       MapTo<Question, QuestionModel>();
            return new ReturnResult<IPagedData<QuestionModel>>(1, res);
        }
        #endregion
    }
}
