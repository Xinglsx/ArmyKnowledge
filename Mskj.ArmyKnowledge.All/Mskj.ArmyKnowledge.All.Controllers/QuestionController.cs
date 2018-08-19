using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using System.Web.Http;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts.DataObj;

namespace Mskj.ArmyKnowledge.All.Controllers
{

    [RoutePrefix("Question")]
    public class QuestionController : BaseController
    {
        #region 构造函数
        private readonly IQuestionService _QuestionService;

        public QuestionController(IQuestionService questionService)
        {
            _QuestionService = questionService;
        }
        #endregion

        #region 问题回答
        /// <summary>
        /// 新增问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [Route("AddQuestion")]
        [HttpPost]
        public object AddQuestion(QuestionModel question)
        {
            return _QuestionService.AddQuestion(question);
        }
        /// <summary>
        /// 更新问题
        /// </summary>
        [Route("UpdateQuestion")]
        [HttpPost]
        public object UpdateQuestion(QuestionModel question)
        {
            return _QuestionService.UpdateQuestion(question);
        }
        /// <summary>
        /// 提交审核问题
        /// </summary>
        [Route("SubmitQuestion")]
        [HttpPost]
        public object SubmitQuestion(QuestionModel question)
        {
            return _QuestionService.SubmitQuestion(question);
        }
        /// <summary>
        /// 审核通过问题
        /// </summary>
        [Route("AuditQuestion")]
        [HttpPost]
        public object AuditQuestion(QuestionModel question)
        {
            return _QuestionService.AuditQuestion(question);
        }
        /// <summary>
        /// 删除问题
        /// </summary>
        [Route("DeleteQuestion")]
        [HttpPost]
        public object DeleteQuestion(PostId Question)
        {
            return _QuestionService.DeleteQuestion(Question.Id);
        }
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sortType">排序规则 0-首页 1-热榜 2-推荐 3-最新</param>
        /// <param name="filter">查询关键字</param>
        /// <returns></returns>
        [Route("GetQuestions")]
        [HttpGet]
        public object GetQuestions(string filter = "",
            int state = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            return _QuestionService.GetQuestions(filter, state,pageIndex, pageSize, sortType);
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
        [Route("GetUserQuestionModels")]
        [HttpGet]
        public object GetUserQuestionModels(string userid,
            string filter = "", int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            return _QuestionService.GetUserQuestionModels(userid,filter, pageIndex, pageSize, sortType);
        }
        /// <summary>
        /// 分页获取问题的回答
        /// </summary>
        /// <param name="questionId">问题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [Route("GetAnswers")]
        [HttpGet]
        public object GetAnswers(
            string questionId, int pageIndex = 1, int pageSize = 10)
        {
            return _QuestionService.GetAnswers(questionId, pageIndex, pageSize);
        }
        /// <summary>
        /// 分页获取对应用户回答的问题
        /// </summary>
        /// <param name="questionId">问题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [Route("GetUserAnswers")]
        [HttpGet]
        public object GetUserAnswers(string userid,
            string questionId, int pageIndex = 1, int pageSize = 10)
        {
            return _QuestionService.GetUserAnswers(userid, questionId, pageIndex, pageSize);
        }
        /// <summary>
        /// 获取一个问题
        /// </summary>
        [Route("GetOneQuestion")]
        [HttpGet]
        public object GetOneQuestion(string questionid)
        {
            return _QuestionService.GetOneQuestion(questionid);
        }
        /// <summary>
        /// 增加阅读数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("UpdateReadCount")]
        [HttpPost]
        public object UpdateReadCount(PostId Question)
        {
            return _QuestionService.UpdateReadCount(Question.Id);
        }
        /// <summary>
        /// 增加点击数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("UpdatePraiseCount")]
        [HttpPost]
        public object UpdatePraiseCount(PostId Question)
        {
            return _QuestionService.UpdatePraiseCount(Question.Id);
        }
        /// <summary>
        /// 增加评论数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("UpdateCommentCount")]
        [HttpPost]
        public object UpdateCommentCount(PostId Question)
        {
            return _QuestionService.UpdateCommentCount(Question.Id);
        }
        /// <summary>
        /// 更新热度
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("UpdateHeatCount")]
        [HttpPost]
        public object UpdateHeatCount(PostId question)
        {
            _QuestionService.UpdateHeatCount(question.Id);
            return new ReturnResult<bool>(1, true);
        }
        /// <summary>
        /// 增加评论
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        [Route("AddAnswer")]
        [HttpPost]
        public object AddAnswer(Answer answer)
        {
            return _QuestionService.AddAnswer(answer);
        }
        #endregion

        #region 最近浏览
        /// <summary>
        /// 增加或更新最近浏览
        /// </summary>
        /// <returns></returns>
        [Route("AddRecord")]
        [HttpPost]
        public object AddRecord(Record record)
        {
            return _QuestionService.AddRecord(record);
        }
        /// <summary>
        /// 查看最近浏览的问题列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        [Route("GetRecordQuestions")]
        [HttpGet]
        public object GetRecordQuestions(string userid,
            int pageIndex = 1, int pageSize = 10, string filter = "")
        {
            return _QuestionService.GetRecordQuestions(userid, pageIndex, pageSize, filter);
        }
        /// <summary>
        /// 增加最近浏览
        /// </summary>
        [Route("AddCollect")]
        [HttpPost]
        public object AddCollect(Record record)
        {
            return _QuestionService.AddCollect(record);
        }
        /// <summary>
        /// 查看我收藏的问题列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        [Route("GetCollectionQuestions")]
        [HttpGet]
        public object GetCollectionQuestions(string userid,
            int pageIndex = 1, int pageSize = 10, string filter = "")
        {
            return _QuestionService.GetCollectionQuestions(userid, pageIndex, pageSize, filter);
        }
        #endregion
    }
}
