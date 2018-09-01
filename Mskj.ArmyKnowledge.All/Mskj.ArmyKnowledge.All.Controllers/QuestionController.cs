using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using System.Web.Http;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Common.DataObj;

namespace Mskj.ArmyKnowledge.All.Controllers
{

    [RoutePrefix("Question")]
    public class QuestionController : BaseController
    {
        #region 构造函数
        private readonly IQuestionService _QuestionService;
        private readonly IUsersService _IUsersService;

        public QuestionController(IQuestionService questionService,IUsersService usersService)
        {
            _QuestionService = questionService;
            _IUsersService = usersService;
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
        public object AddQuestion(PostQuestion question)
        {
            if(question == null || string.IsNullOrEmpty(question.Author) || 
                string.IsNullOrEmpty(question.Content) || string.IsNullOrEmpty(question.Title))
            {
                return new ReturnResult<QuestionModel>(-4, "传入参数错误!");
            }
            return _QuestionService.AddQuestion(question.ToModel());
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
        public object SubmitQuestion(PostId question)
        {
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<QuestionModel>(-4, "传入参数错误!");
            }
            return _QuestionService.SubmitQuestion(question.Id);
        }
        /// <summary>
        /// 新增问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [Route("SaveAndSubmitQuestion")]
        [HttpPost]
        public object SaveAndSubmitQuestion(PostQuestion question)
        {
            if (question == null || string.IsNullOrEmpty(question.Author) ||
                string.IsNullOrEmpty(question.Content) || string.IsNullOrEmpty(question.Title))
            {
                return new ReturnResult<QuestionModel>(-4, "传入参数错误!");
            }
            var temp = question.ToModel();
            temp.QuestionState = 1;
            return _QuestionService.AddQuestion(temp);
        }
        /// <summary>
        /// 审核通过问题
        /// </summary>
        [Route("AuditQuestion")]
        [HttpPost]
        public object AuditPassQuestion(PostId question)
        {
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<QuestionModel>(-4, "传入参数错误!");
            }
            return _QuestionService.AuditQuestion(question.Id);
        }
        /// <summary>
        /// 审核不通过问题
        /// </summary>
        [Route("AuditFailQuestion")]
        [HttpPost]
        public object AuditFailQuestion(PostId question)
        {
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<QuestionModel>(-4, "传入参数错误!");
            }
            return _QuestionService.AuditFailQuestion(question.Id);
        }
        /// <summary>
        /// 删除问题
        /// </summary>
        [Route("DeleteQuestion")]
        [HttpPost]
        public object DeleteQuestion(PostId question)
        {
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<bool>(-2, "参数传入错误");
            }
            return _QuestionService.DeleteQuestion(question.Id);
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
        public object GetQuestions(string userid = "", string filter = "",
            int state = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            return _QuestionService.GetQuestions(userid,filter, state,pageIndex, pageSize, sortType);
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
        [Route("GetUserQuestion")]
        [HttpGet]
        public object GetUserQuestion(string userid,
            string filter = "", int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            return _QuestionService.GetUserQuestion(userid,filter, pageIndex, pageSize, sortType);
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
            return _QuestionService.GetAnswers("",questionId, pageIndex, pageSize);
        }
        /// <summary>
        /// 分页获取问题的回答
        /// </summary>
        /// <param name="questionId">问题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [Route("GetAllAnswers")]
        [HttpGet]
        public object GetAllAnswers(string filter = "", 
            int pageIndex = 1, int pageSize = 10)
        {
            return _QuestionService.GetAnswers(filter, "all", pageIndex, pageSize);
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
        public object GetUserAnswers(string userid, int pageIndex = 1, int pageSize = 10)
        {
            return _QuestionService.GetUserAnswers(userid, pageIndex, pageSize);
        }
        /// <summary>
        /// 获取一个问题
        /// </summary>
        [Route("GetOneQuestion")]
        [HttpGet]
        public object GetOneQuestion(string questionid,int pageIndex = 1, int pageSize = 10)
        {
            return _QuestionService.GetOneQuestion(questionid, pageIndex, pageSize);
        }
        /// <summary>
        /// 增加阅读数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("UpdateReadCount")]
        [HttpPost]
        public object UpdateReadCount(PostId question)
        {
            return _QuestionService.UpdateReadCount(question.Id);
        }
        /// <summary>
        /// 增加点赞数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("UpdatePraiseCount")]
        [HttpPost]
        public object UpdatePraiseCount(PostId question)
        {
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<QuestionModel>(-4, "传入参数错误!");
            }
            return _QuestionService.UpdatePraiseCount(question.Id);
        }
        /// <summary>
        /// 增加评论数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("UpdateCommentCount")]
        [HttpPost]
        public object UpdateCommentCount(PostId question)
        {
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<QuestionModel>(-4, "传入参数错误!");
            }
            return _QuestionService.UpdateCommentCount(question.Id);
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
            if (question == null || string.IsNullOrEmpty(question.Id))
            {
                return new ReturnResult<QuestionModel>(-4, "传入参数错误!");
            }
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
        /// <summary>
        /// 删除评论
        /// </summary>
        [Route("DeleteAnswer")]
        [HttpPost]
        public object DeleteAnswer(PostId answer)
        {
            if (answer == null || string.IsNullOrEmpty(answer.Id))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _QuestionService.DeleteAnswer(answer.Id);
        }
        #endregion

        #region 最近浏览
        /// <summary>
        /// 增加或更新最近浏览
        /// </summary>
        /// <returns></returns>
        [Route("AddRecord")]
        [HttpPost]
        public object AddRecord(PostRecord record)
        {
            if (record == null || string.IsNullOrEmpty(record.QuestionId)
                || string.IsNullOrEmpty(record.UserId))
            {
                return new ReturnResult<bool>(-2, "参数传入错误!");
            }
            Record saveTemp = _QuestionService.GetRecord(record.QuestionId, record.UserId);
            bool isNew = false;
            if (saveTemp == null)
            {
                saveTemp = record.ToModel();
                isNew = true;
            }
            switch (record.Type)
            {
                default:
                case 0:
                    break;
                case 1://点赞
                    saveTemp.ispraise = true;
                    break;
                case 2://收藏
                    saveTemp.iscollect = true;
                    break;
                case -1://取消点赞
                    saveTemp.ispraise = false;
                    break;
                case -2://取消收藏
                    saveTemp.iscollect = false;
                    break;
            }
            ReturnResult<bool> res = new ReturnResult<bool>();
            if (isNew)
            {
                res = _QuestionService.AddRecord(saveTemp);
            }
            else
            {
                res = _QuestionService.UpdateRecord(saveTemp);
            }
            if (res.code > 0)
            {
                switch (record.Type)
                {
                    default:
                    case 0:
                        break;
                    case 1://点赞
                        _QuestionService.UpdatePraiseCount(record.QuestionId, 1);
                        _IUsersService.UpdateCollectCount(record.UserId, 1);
                        break;
                    case 2://收藏
                        _QuestionService.UpdateCollectCount(record.QuestionId, 1);
                        break;
                    case -1://取消点赞
                        _QuestionService.UpdatePraiseCount(record.QuestionId, -1);
                        break;
                    case -2://取消收藏
                        _QuestionService.UpdateCollectCount(record.QuestionId, -1);
                        _IUsersService.UpdateCollectCount(record.UserId, -1);
                        break;
                }
            }
            return res;
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
