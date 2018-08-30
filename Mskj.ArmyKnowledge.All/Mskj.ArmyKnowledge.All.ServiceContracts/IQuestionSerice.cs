using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using Mskj.ArmyKnowledge.All.Common.DataObj;
using Mskj.ArmyKnowledge.All.Common.PostData;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IQuestionService : IBaseService<QuestionModel>,IServiceContract
    {
        #region 问题回答
        /// <summary>
        /// 新增问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        ReturnResult<QuestionModel> AddQuestion(QuestionModel question);
        /// <summary>
        /// 更新问题
        /// </summary>
        ReturnResult<bool> UpdateQuestion(QuestionModel question);
        /// <summary>
        /// 提交审核问题
        /// </summary>
        ReturnResult<bool> SubmitQuestion(string id);
        /// <summary>
        /// 审核通过问题
        /// </summary>
        ReturnResult<bool> AuditQuestion(string id);
        /// <summary>
        /// 审核不通过问题
        /// </summary>
        ReturnResult<bool> AuditFailQuestion(string id);
        /// <summary>
        /// 删除问题
        /// </summary>
        ReturnResult<bool> DeleteQuestion(string id);
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sortType">排序规则 0-首页 1-热榜 2-推荐 3-最新</param>
        /// <param name="filter">查询关键字</param>
        /// <returns></returns>
        ReturnResult<IPagedData<QuestionModel>> GetQuestions(string filter = "",
            int state = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0);
        /// <summary>
        /// 分页获取用户对应的问题列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        ReturnResult<IPagedData<QuestionModel>> GetUserQuestion(string userid,
            string filter = "", int pageIndex = 1, int pageSize = 10, int sortType = 3);
        /// <summary>
        /// 分页获取问题的回答
        /// </summary>
        /// <param name="questionId">问题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        ReturnResult<IPagedData<AnswerModel>> GetAnswers(
            string questionId, int pageIndex = 1, int pageSize = 10);
        /// <summary>
        /// 分页获取对应用户回答的问题
        /// </summary>
        /// <param name="questionId">问题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        ReturnResult<IPagedData<Question>> GetUserAnswers(string userid,int pageIndex = 1, int pageSize = 10);
        /// <summary>
        /// 获取一个问题
        /// </summary>
        ReturnResult<QuestionModel> GetOneQuestion(string questionId, int pageIndex = 1, int pageSize = 10);
        /// <summary>
        /// 增加阅读数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        ReturnResult<bool> UpdateReadCount(string questionId);
        /// <summary>
        /// 增加点击数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        ReturnResult<bool> UpdatePraiseCount(string questionId);
        /// <summary>
        /// 增加评论数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        ReturnResult<bool> UpdateCommentCount(string questionId);
        /// <summary>
        /// 更新热度
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        void UpdateHeatCount(string questionId);
        /// <summary>
        /// 增加评论
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        ReturnResult<bool> AddAnswer(Answer answer);
        #endregion

        #region 最近浏览
        /// <summary>
        /// 增加或更新最近浏览
        /// </summary>
        /// <returns></returns>
        ReturnResult<bool> AddRecord(Record record);
        /// <summary>
        /// 查看最近浏览的问题列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        ReturnResult<IPagedData<QuestionModel>> GetRecordQuestions(string userid,
            int pageIndex = 1, int pageSize = 10, string filter = "");
        /// <summary>
        /// 增加最近浏览
        /// </summary>
        ReturnResult<bool> AddCollect(Record record);
        /// <summary>
        /// 查看我收藏的问题列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        ReturnResult<IPagedData<QuestionModel>> GetCollectionQuestions(string userid,
            int pageIndex = 1, int pageSize = 10, string filter = "");
        #endregion
        
    }
}
