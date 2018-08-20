﻿using Mskj.ArmyKnowledge.All.ServiceContracts;
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

namespace Mskj.ArmyKnowledge.All.Services
{
    public class QuestionService : BaseService<QuestionModel, Question>, IQuestionService
    {
        #region 构造函数
        private readonly IRepository<Question> _QuestionRepository;
        private readonly IRepository<Answer> _AnswerDetailRepository;
        private readonly IRepository<Record> _RecordRepository;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        public QuestionService(IRepository<Question> questionRepository,
            IRepository<Answer> answerDetailRepository,
            IRepository<Record> recordRepository) :
            base(questionRepository)
        {
            this._QuestionRepository = questionRepository;
            this._AnswerDetailRepository = answerDetailRepository;
            this._RecordRepository = recordRepository;
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
                return new ReturnResult<QuestionModel>(-1, exp.Message);
            }
            if(res)
            {
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
                res = this.Update(question);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, false, exp.Message);
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
        public ReturnResult<bool> SubmitQuestion(QuestionModel question)
        {
            if (question.QuestionState != 0)
            {
                return new ReturnResult<bool>(-2, "问题状态不是[新建状态]！");
            }
            question.QuestionState = 1;
            return this.UpdateQuestion(question);
        }
        /// <summary>
        /// 审核通过问题
        /// </summary>
        public ReturnResult<bool> AuditQuestion(QuestionModel question)
        {
            if(question.QuestionState != 1)
            {
                return new ReturnResult<bool>(-2, "问题状态不是[提交审核状态]！");
            }
            question.QuestionState = 2;
            return this.UpdateQuestion(question);
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
                return new ReturnResult<bool>(-1, exp.Message);
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
        /// <param name="pageSize">每页条数</param>
        /// <param name="sortType">排序规则 0-首页 1-热榜 2-推荐 3-最新</param>
        /// <param name="filter">查询关键字</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<QuestionModel>> GetQuestions(string filter = "",
            int state = 2,int pageIndex = 1,int pageSize = 10, int sortType = 0)
        {
            Expression<Func<QuestionModel, bool>> expression;
            if (string.IsNullOrEmpty(filter))
            {
                expression = x => x.QuestionState == state;
            }
            else
            {
                expression = x => x.QuestionState == state && (
                    x.Title.Contains(filter) || x.AuthorNickname.Contains(filter) ||
                    x.Introduction.Contains(filter) || x.Content.Contains(filter)
                    );

            }
            return GetBaseQuestionModels(pageIndex, pageSize, sortType, expression);
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
        public ReturnResult<IPagedData<QuestionModel>> GetUserQuestionModels(string userid,
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
        /// <param name="sortType">排序方式 0-综合排序 1-最新发布</param>
        /// <param name="expression">查询表达示</param>
        /// <returns></returns>
        private ReturnResult<IPagedData<QuestionModel>> GetBaseQuestionModels(int pageIndex,
            int pageSize, int sortType, Expression<Func<QuestionModel, bool>> expression)
        {
            List<SortInfo<QuestionModel>> sorts = new List<SortInfo<QuestionModel>>();
            SortInfo<QuestionModel> sort;
            switch (sortType)
            {
                case 0:
                    sort = new SortInfo<QuestionModel>(p => new { p.Publishtime },
                        SortOrder.Descending);
                    break;
                case 1:
                    sort = new SortInfo<QuestionModel>(p => new { p.HeatCount },
                        SortOrder.Descending);
                    break;
                case 2:
                    sort = new SortInfo<QuestionModel>(p => new { p.CommentCount },
                        SortOrder.Descending);
                    break;
                case 3:
                default:
                    sort = new SortInfo<QuestionModel>(p => new { p.Publishtime },
                        SortOrder.Descending);
                    break;
            }
            sorts.Add(sort);
            sorts.Add(new SortInfo<QuestionModel>(p => new { p.Publishtime }, SortOrder.Descending));
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
        public ReturnResult<IPagedData<Answer>> GetAnswers(
            string questionId,int pageIndex = 1,int pageSize = 10)
        {
            IQueryable<Answer> query = _AnswerDetailRepository.Find().OrderByDescending(p => p.publishtime);
            var res = query.ToPage(pageIndex, pageSize);
            return new ReturnResult<IPagedData<Answer>>(1, res);
        }
        /// <summary>
        /// 分页获取对应用户回答的问题
        /// </summary>
        /// <param name="questionId">问题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Question>> GetUserAnswers(string userid,
            string questionId, int pageIndex = 1, int pageSize = 10)
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
        public ReturnResult<QuestionModel> GetOneQuestion(string questionId)
        {
            QuestionModel question = this.GetOne(p => p.Id == questionId);
            if(question != null)
            {
                question.Answers = _AnswerDetailRepository.Find().OrderByDescending(p => p.publishtime).
                    Take(10).ToList();
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
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
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
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
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
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
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
        /// <param name="questionId">问题ID</param>
        /// <returns></returns>
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
        /// <param name="question">问题对象</param>
        /// <returns></returns>
        public void UpdateHeatCount(QuestionModel question)
        {
            question.HeatCount = 1000 + question.ReadCount;
            question.HeatCount += question.PraiseCount * 5;
            question.HeatCount += question.CommentCount * 10;
            //距离现在的时间越长，减分也越大。
            int hours = (DateTime.Now - question.Publishtime).Hours;
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
        /// <param name="answer"></param>
        /// <returns></returns>
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
                return new ReturnResult<bool>(-1, exp.Message);
            }
            if (res)
            {
                //评论增加成功时，更新主表。
                UpdateCommentCount(answer.questionid);
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
                    return new ReturnResult<bool>(-1, exp.Message);
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
                    return new ReturnResult<bool>(-1, exp.Message);
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