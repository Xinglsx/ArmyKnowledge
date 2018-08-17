using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using Mskj.ArmyKnowledge.All.ServiceContracts.DataObj;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IQuestionService : IBaseService<QuestionModel>,IServiceContract
    {
        /// <summary>
        /// 新增问题
        /// </summary>
        ReturnResult<QuestionModel> AddQuestion(QuestionModel question);
        /// <summary>
        /// 更新问题
        /// </summary>
        /// <param name="cert">用户认证信息</param>
        ReturnResult<bool> UpdateQuestion(QuestionModel question);
    }
}
